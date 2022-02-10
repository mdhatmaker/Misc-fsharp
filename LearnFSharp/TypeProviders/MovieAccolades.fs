module TypeProviders.MovieAccolades

open System
open FSharp.Data

// Install-Package FSharp.Data

type AccoladeData = HtmlProvider<"https://en.wikipedia.org/wiki/List_of_accolades_received_by_Spotlight_(film)">

let spotlightData = AccoladeData.Load("https://en.wikipedia.org/wiki/List_of_accolades_received_by_Spotlight_(film)")

let awardNumbers (data: AccoladeData) =
    data.Tables.AccoladesEdit.Rows
    |> Seq.filter (fun row -> row.Result = "Won")
    |> Seq.groupBy (fun row -> row.``Recipient(s)``)
    //|> Seq.groupBy (fun row -> row.``Recipient(s) and nominee(s)``)
    |> Seq.map (fun (person, awards) -> (person, Seq.length awards))
    |> Seq.sortByDescending (fun (person, count) -> count)



for (person, count) in awardNumbers spotlightData do
    printfn "%s,%d" person count



let urls = [
    "https://en.wikipedia.org/wiki/List_of_accolades_received_by_Spotlight_(film)"
    "https://en.wikipedia.org/wiki/List_of_accolades_received_by_Moonlight_(2016_film)"
    "https://en.wikipedia.org/wiki/List_of_accolades_received_by_La_La_Land_(film)"
]

let allMovies =
    urls
    |> Seq.map AccoladeData.AsyncLoad
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Seq.map awardNumbers


for movie in allMovies do
    for (p,c) in movie do
        printfn "%s,%d" p c
    printfn ""


