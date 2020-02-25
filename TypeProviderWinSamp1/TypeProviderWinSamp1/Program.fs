// Learn more about F# at https://fsharp.org
// See the 'F# Tutorial' project for more help.

open MySettings
open MyJson
open MySql

[<EntryPoint>]
let main argv =
    //printfn "%A" argv

    (*let sqlData = dbSchema.GetDataContext()

    SqlData.Purple
    |> Seq.toList
    |> PrintPurple*)

    
    printfn "%f" SettingsData.Keyf1
    printfn "%s" SettingsData.Keys
    printfn "%i" SettingsData.Keyi
    printfn "%f" SettingsData.Keyf2
    printfn "%b" SettingsData.Keyb

    printfn "---------------------------------------------"

    //let data = JsonData.GetSample()
    //PrintJson data

    (*let data = JsonData.GetSamples()
    Array.toList data
    |> PrintJsonArray*)

    let data = JsonData.Load("https://localhost:44323/api/values/")
    Array.toList data
    |> PrintJsonArray

    printfn "---------------------------------------------"



    System.Console.Write("Press any key... ")
    System.Console.ReadKey() |> ignore

    0 // return an integer exit code
