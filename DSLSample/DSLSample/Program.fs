// Learn more about F# at http://fsharp.org

open System
open FSharp.Data.UnitSystems.SI.UnitSymbols

type Precipitation =
    | Rain
    | Snow
    | Hail
    
 type Weather =
    | Sunny
    | Precipitation of Precipitation
    | Windy of float<m/s>

let printWeather weather =
    match weather with
    | Sunny -> printfn "Sunny weather!"
    | Precipitation p -> printfn "Weather with %A" p
    | Windy speed -> printfn "Wind speed: %.1f m/s" speed

let sunnyWeather = Sunny
let rainyWeather = Precipitation Rain
let windyWeather = Windy 15.0<m/s>

type Person =
    { FirstName: string
      LastName: string
      Age: int }

let person =
    { FirstName = "Mikhail"
      LastName = "Smal"
      Age = 30 }


[<EntryPoint>]
let main argv =
    printfn "***** STARTING F# APP *****"
    printfn "%A" sunnyWeather
    printfn "%A" rainyWeather
    printfn "%A" windyWeather
    printWeather sunnyWeather
    printfn "%A" person
    
    Console.WriteLine("Done...Press ENTER...")
    Console.ReadLine() |> ignore

    0 // return an integer exit code
