// Learn more about F# at http://fsharp.org

open System

open Settings


[<EntryPoint>]
let main argv =
    printfn "----- F# TypeProviders -----\n"

    

    Settings.demo()
    WorldBank.demo()




    printfn("\n\n----------- Done -----------")
    System.Console.Read() |> ignore
    System.Threading.Thread.Sleep(5000)
    0 // return an integer exit code
