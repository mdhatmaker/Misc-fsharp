module Settings

open FSharp.Configuration

type Settings = AppSettings<"app.config">


let demo() =
    printfn "%s START.....\n" __SOURCE_FILE__

    printfn "%f" Settings.Keyf1
    printfn "%b" Settings.Keyb
    printfn "%i" Settings.Keyi
    printfn "%s" Settings.Keys
    printfn "%f" Settings.Keyf2



    printfn "\n.....%s END." __SOURCE_FILE__

