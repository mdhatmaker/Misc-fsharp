module TpWorldBank



open FSharp.Data
open FSharp.Charting


let demo() =
    printfn "%s START.....\n" __SOURCE_FILE__

    let data = WorldBankData.GetDataContext()

    let d1 =
        data.Countries.``United Kingdom``
            .Indicators.``Gross capital formation (% of GDP)``

    let d2 =
        data.Countries.Africa
            .Indicators.``Gross capital formation (% of GDP)``
        
    let d3 =
        data.Countries.China
            .Indicators.``Gross capital formation (% of GDP)``

    let d1b =
        data.Countries.``United Kingdom``
            .Indicators.``Gross capital formation (% of GDP)``
        //|> Seq.maxBy fst
        |> Seq.take 3
        |> Seq.iter (printfn "%A")

    let chart1 =
        data.Countries.``United Kingdom``
            .Indicators.``Gross capital formation (% of GDP)``
        |> Chart.Line
        
    (*data.Countries.``United Kingdom``
        .Indicators.``Gross capital formation (% of GDP)``
        |> Chart.Line*)

    let expectedIncome =
        d1
        |> Seq.take 8

    let expectedExpenses =
        d2
        |> Seq.take 8

    let computedProfit =
        d3
        |> Seq.take 8

    Chart.Combine(
       [ Chart.Line(expectedIncome,Name="Income")
         Chart.Line(expectedExpenses,Name="Expenses") 
         Chart.Line(computedProfit,Name="Profit") ]) |> ignore



    printfn "\n.....%s END." __SOURCE_FILE__
