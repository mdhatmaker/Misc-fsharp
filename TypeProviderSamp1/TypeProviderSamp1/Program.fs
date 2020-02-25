// Learn more about F# at http://fsharp.org

open System

open Settings


[<EntryPoint>]
let main argv =
    printfn "%f" Settings.Keyf1
    printfn "%b" Settings.Keyb
    printfn "%i" Settings.Keyi
    printfn "%s" Settings.Keys
    printfn "%f" Settings.Keyf2

    System.Console.ReadKey() |> ignore

    0 // return an integer exit code
