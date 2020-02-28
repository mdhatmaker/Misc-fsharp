module MySettings

open FSharp.Configuration


type SettingsData = AppSettings<"app.config">



let tryMe() =
    printfn "%f" SettingsData.Keyf1
    printfn "%s" SettingsData.Keys
    printfn "%i" SettingsData.Keyi
    printfn "%f" SettingsData.Keyf2
    printfn "%b" SettingsData.Keyb
    
    printfn "---------------------------------------------"

