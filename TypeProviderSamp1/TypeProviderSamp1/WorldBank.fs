module WorldBank


open FSharp.Data
open FSharp.Charting


let demo() =
    printfn "%s START.....\n" __SOURCE_FILE__

    let data = WorldBankData.GetDataContext()

    let d1 =
        data.Countries.``United Kingdom``
            .Indicators.``Gross capital formation (% of GDP)``
        |> Seq.maxBy fst

    let d2 =
        data.Countries.``United Kingdom``
            .Indicators.``Gross capital formation (% of GDP)``
        |> Chart.Line
        




    printfn "\n.....%s END." __SOURCE_FILE__

