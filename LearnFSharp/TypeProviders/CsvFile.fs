module TypeProviders.CsvFile

open System
open FSharp.Data

// Install-Package FSharp.Data

type AccoladeData = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_accolades_received_by_Spotlight_(film)">

let spotlightData = AccoladeData.Load("https://en.wikipedia.org/wiki/List_of_accolades_received_by_Spotlight_(film)")



