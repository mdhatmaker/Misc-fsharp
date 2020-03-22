module MortgageLending

open System.Collections.Generic


// US government yield curve
// https://www.quandl.com/api/v3/datasets/USTREASURY/YIELD.csv?api_key=YOURAPIKEY



let demo () =
    /// Get Quandl data AND display XPlot chart:
    let getData dbTuple =
        let result = QuandlModule.Quandl.GetTimeseries dbTuple
        let dataList:List<obj[]> = result.DatasetData.Data
        printfn "COLUMNS....%A\nROW COUNT....%i\n" result.DatasetData.ColumnNames dataList.Count |> ignore
        dataList |> Seq.toList

    // get last n from given list
    let rec last n xs =
        if List.length xs <= n then xs
        else last n xs.Tail

    let dff_data = getData ("FRED", "DFF")      // Effective Federal Funds Rate
    let dt3_data = getData ("FRED", "DTB3")     // 3-Month Treasury Bill: Secondary Market Rate
    let dt5_data = getData ("FRED", "DGS5")     // 5-Year Treasury Constant Maturity Rate
    let dt10_data = getData ("FRED", "DGS10")   // 10-Year Treasury Constant Maturity Rate
    let dt30_data = getData ("FRED", "DGS30")   // 30-Year Treasury Constant Maturity Rate
    let dted_data = getData ("FRED", "TEDRATE")         // TED Spread
    let dprime_data = getData ("FRED", "DPRIME")        // Bank Prime Loan Rate
    let dyield_data = getData("USTREASURY", "YIELD")    // US government yield curve

    let count = 750

    let li1 = List.take count dff_data
    let li2 = List.take count dt3_data
    let li3 = List.take count dt5_data
    let li4 = List.take count dt10_data
    let li5 = List.take count dt30_data
    let li6 = List.take count dted_data
    let li7 = List.take count dprime_data
    let li8 = List.take count dyield_data

    printfn "%A" li1
    printfn "%A" li2
    printfn "%A" li3
    printfn "%A" li4
    printfn "%A" li5
    printfn "%A" li6
    printfn "%A" li7
    printfn "%A" li8

    printfn "Done."



(*
T5YIE
5-year Breakeven Inflation Rate
T10YIE
10-year Breakeven Inflation Rate
T5YIFR
5-Year, 5-Year Forward Inflation Expectation Rate
*)