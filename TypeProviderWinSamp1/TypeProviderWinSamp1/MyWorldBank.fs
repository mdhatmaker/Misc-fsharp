module MyWorldBank

/// https://fsharp.github.io/FSharp.Data/library/WorldBank.html
/// https://fsharp.github.io/FSharp.Data/library/JsonValue.html

open FSharp.Data



let tryMe() =

    let data = WorldBankData.GetDataContext()

    let d1 = 
        data
            .Countries.``United Kingdom``
            .Indicators.``Gross capital formation (% of GDP)``
        |> Seq.maxBy fst
    

    let d2 =
        data
            .Countries.``United States``
            .Indicators.``Access to clean fuels and technologies for cooking (% of population)``
        |> Seq.toList
        //|> printfn "%A"

    printfn "%A" d1 

    printfn "%A" d2

    let countries =
        data.Countries
        |> Seq.toList

    printfn "%A" countries







    printfn "----------------------------------------------------------------------------"