// Learn more about F# at http://fsharp.org

open System
open Microsoft.AspNetCore.SignalR.Client

let loginResponseHandler (connection :HubConnection) (success :bool) (id :string) =
  printfn "loginResponseHandler"

let messageHandler (message :string) =
  printfn "messageHandler"

let gameStateHandler (connection :HubConnection) (gameState :string) =
  printfn "gameStateHandler"
  //connection.InvokeAsync("Turn", playerId, move.ToString()) |> ignore

let rec reconnect (connection :HubConnection) (error :'a) = 
  printfn "reconnect"


[<EntryPoint>]
let main argv =
    // Create connection to game server
    let connection =
        (HubConnectionBuilder())
            .WithUrl("http://localhost:5000/gameHub")
            .Build()

    // Event handlers
    connection.On<bool, string>("LoginResponse", fun success id -> loginResponseHandler connection success id) |> ignore
    connection.On<string>("Message", fun message -> messageHandler message) |> ignore
    connection.On<string>("GameState", fun gameState -> gameStateHandler connection gameState) |> ignore
    //connection.add_Closed(fun error -> reconnect connection error)
    //connection.On<string>("Closed", fun error -> printfn "%A" error)
    connection.On<string>("Closed", fun error -> reconnect connection error) |> ignore

    let myName = "Ready Player One!"

    // Start connection and login
    try
        connection.StartAsync().Wait()
        connection.InvokeAsync("Login", myName).Wait()
    with
    | ex -> printfn "Connection error %s" (ex.ToString())
            Environment.Exit(1)

    let getCommand hubConnection =
        printfn "getCommand: %A" hubConnection

    // List for 'q' to quit
    getCommand connection




    printfn "Done."
    0 // return an integer exit code









    (*/////////////////////// FURTHER RESEARCH //////////////////////
    https://github.com/Thorium/SignalR-FSharp-Example/tree/master/SignalRServer

    *)

    (*/////////////////////// CODE GRAVEYARD ////////////////////////

    *)