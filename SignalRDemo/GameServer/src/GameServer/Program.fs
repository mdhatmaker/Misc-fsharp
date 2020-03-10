module GameServer.App

/// https://www.codesuji.com/2019/02/19/Building-Game-with-SignalR-and-F/


open System
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open System.Threading
open System.Threading.Tasks
open Microsoft.AspNetCore.SignalR
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Http

// ---------------------------------
// Models
// ---------------------------------

type Message =
    {
        Text : string
    }

type GameState =
    {
        Description : string
        State : string
    }

type IClientApi =
    abstract member LoginResponse :bool * string -> System.Threading.Tasks.Task
    abstract member Message :string -> System.Threading.Tasks.Task
    abstract member GameState :string -> System.Threading.Tasks.Task

type GameHub () =
    inherit Hub<IClientApi> ()

    let addPlayer name =
        printfn "addPlayer: %s" name
        (true, "playerid_1")

    let removePlayer playerId =
        printfn "removePlayer:"

    let updatePlayerDirection playerId direction =
        printfn "updatePlayerDirection: %s %s"
    /// Accept client logins
    member this.Login (name:string) =
        let connectionId = this.Context.ConnectionId
        let success, playerId = addPlayer name
        if success then
            // Tell client login success and their playerId
            this.Clients.Client(connectionId).LoginResponse(true, playerId)
            // Tell clients of new player
            this.Clients.All.Message(sprintf "New Player: %s (%s)" name playerId)
        else
            // Tell client login failed
            this.Clients.Client(connectionId).LoginResponse(false, "") 

    /// Handle client logout
    member this.Logout (playerId:string) =
        removePlayer playerId
        // Tell clients of player logout
        this.Clients.All.Message(sprintf "Player left: %s" playerId)

    /// Handle player changing direction
    member this.Turn (playerId:string, direction:string) =
        updatePlayerDirection playerId direction

    /// Pass along message from one client to all clients
    member this.Send (message:string) =
        this.Clients.All.Message(message)

type GameService (hubContext :IHubContext<GameHub,IClientApi>) =
    inherit BackgroundService ()

    let gState = { Description="game state description"; State="GAME_STATE"}

    let TurnFrequency = 2000.
    let updateState () =
        printfn "updateState:"
    let serializeGameState gState :string =
        printfn "serializeGameState: %A" gState
        gState.ToString()
        
    member this.HubContext :IHubContext<GameHub, IClientApi> = hubContext

    override this.ExecuteAsync (stoppingToken :CancellationToken) =
        let pingTimer = new System.Timers.Timer(TurnFrequency)
        pingTimer.Elapsed.Add(fun _ ->
            updateState ()
            let stateSerialized = serializeGameState gState
            this.HubContext.Clients.All.GameState(stateSerialized) |> ignore)

        pingTimer.Start()
        Task.CompletedTask
          




// ---------------------------------
// Views
// ---------------------------------

module Views =
    open GiraffeViewEngine

    let layout (content: XmlNode list) =
        html [] [
            head [] [
                title []  [ encodedText "GameServer" ]
                link [ _rel  "stylesheet"
                       _type "text/css"
                       _href "/main.css" ]
            ]
            body [] content
        ]

    let partial () =
        h1 [] [ encodedText "GameServer" ]

    let index (model : Message) =
        [
            partial()
            p [] [ encodedText model.Text ]
        ] |> layout


module ClientViews =
    open GiraffeViewEngine

    let layout (content: XmlNode list) =
        html [] [
            head [] [
                title []  [ encodedText "SnakeWorld" ]
                link [ _rel  "stylesheet"
                       _type "text/css"
                       _href "/main.css" ]
            ]
            body [] content
        ]

    let index (model : Message) =
      [
        div [ _class "container"] [ 
          div [ _class "row" ] [
            div [ _id "mapWrapper"; _class "col-6" ] [
              canvas [ _id "worldMap"; _class "world-map"; _width "200"; _height "200" ] [];
              div [ _id "playerList"; _class "player-list" ] []
            ]
          ]
          div [ _class "row" ] [
            div [ _id "message"; _class "col-6" ] []
          ]
          div [ _class "row" ] [
            div [ _id "currentState"; _class "col-6" ] []
          ]
        ]
        script [ _src "signalr.js" ] []
        script [ _src "jquery-3.3.1.min.js" ] []
        script [ _src "game-viewer.js" ] []
      ] |> layout


// ---------------------------------
// Web app
// ---------------------------------

let indexHandler (name : string) =
    let greetings = sprintf "Hello %s, from Giraffe!" name
    let model     = { Text = greetings }
    let view      = Views.index model
    htmlView view

let webApp =
    choose [
        GET >=>
            choose [
                route "/" >=> indexHandler "world"
                routef "/hello/%s" indexHandler
            ]
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
    | true  -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseStaticFiles()
        .UseSignalR(fun routes -> routes.MapHub<GameHub>(PathString "/gameHub"))  // SignalR
        .UseGiraffe(webApp)

let configureServices (services : IServiceCollection) =
    services.AddCors()    |> ignore
    services.AddSignalR() |> ignore         // SignalR
    services.AddGiraffe() |> ignore
    services.AddHostedService<GameService>() |> ignore  // GameService

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    let contentRoot = Directory.GetCurrentDirectory()
    let webRoot     = Path.Combine(contentRoot, "WebRoot")
    WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(contentRoot)
        .UseIISIntegration()
        .UseWebRoot(webRoot)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0