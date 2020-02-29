module QuandlModule

/// https://github.com/lppkarl/Quandl.NET
/// https://fsharp.github.io/FSharp.Data/library/JsonValue.html
/// https://stackoverflow.com/questions/41743602/correctly-awaiting-in-f-an-async-c-sharp-method-with-return-type-of-taskt

open System
open Quandl.NET
open System.Collections.Generic

open QuandlTypes


// Print a list of rows of data
let processData fnParse title displayCount data =
    printfn "%s" title
    let li = List.ofSeq data            // use List.ofSeq to convert 'List<T>' to 'T list'
    li
    |> List.take (Math.Min(displayCount, li.Length))
    //|> List.map (fun x -> printfn "%A" x; parseContinuousData x)
    |> List.map fnParse
    |> List.iter (fun x -> printfn "%s" (x.ToString()))
    //|> List.iter (printfn "%A")

// find a specific date (where dateStr like "2018-03-19"
let displayRowWithDate fnParse (data : List<obj[]>) dateStr =
    data
    |> List.ofSeq 
    |> List.filter (fun x -> (string x.[0]) = dateStr)      // first column = date
    |> List.map fnParse
    |> printfn "%A"


module Quandl =
    
    let quandlApiKey = "gCbpWopzuxctHw6y-qq5"
    let client = new QuandlClient(quandlApiKey)

    (*// Add the required settings to pull down data:
    let settings = new Dictionary<string, string>();
    settings.Add("collapse", "weekly");
    settings.Add("trim_start", "2010-02-01");
    settings.Add("trim_end", "2010-04-28");
    settings.Add("transformation", "normalize");
    settings.Add("sort_order", "asc");
    // Fetch:
    //IList<CsvFinancialFormat> data = myQuandl.GetData<CsvFinancialFormat>("YAHOO/MX_IBM", settings); //"GOOG/NYSE_IBM"
    //let data = myQuandl.GetData<CsvFinancialFormat>("YAHOO/MX_IBM", settings); //"GOOG/NYSE_IBM"
    *)

    /// Quandl: Get time-series data
    let GetTimeseries dbTuple =
        let asyncFunction dbTuple =
            client.Timeseries.GetDataAsync(fst dbTuple, snd dbTuple)
            |> Async.AwaitTask
            |> Async.RunSynchronously
        asyncFunction dbTuple

    /// TODO: convert the test values into function arguments
    /// Quandl: Get filtered time-series data
    let GetFilteredTimeseries dbTuple =
        let asyncFunction dbTuple =
            client.Timeseries.GetDataAsync(fst dbTuple, snd dbTuple,
                                            columnIndex=Nullable<int>(4),
                                            startDate=Nullable<DateTime>(DateTime(2014,1,1)),
                                            endDate=Nullable<DateTime>(DateTime(2014,12,31)),
                                            collapse=Nullable<Collapse>(Collapse.Monthly),
                                            transform=Nullable<Transform>(Transform.Rdiff))
            |> Async.AwaitTask
            |> Async.RunSynchronously
        asyncFunction dbTuple

    /// Quandl: Get time-series metadata
    let GetTimeseriesMetadata dbTuple =
        let asyncFunction dbTuple =
            client.Timeseries.GetMetadataAsync(fst dbTuple, snd dbTuple)
            |> Async.AwaitTask
            |> Async.RunSynchronously
        asyncFunction dbTuple

    /// Quandl: Get database metadata (where dbCode like "CHRIS", "WIKI", etc.)
    let GetDatabaseMetadata dbCode =
        let asyncFunction dbCode =
            client.Timeseries.GetDatabaseMetadataAsync(dbCode)
            |> Async.AwaitTask
            |> Async.RunSynchronously
        asyncFunction dbCode

    /// TODO: get data and metadata, get entire time-series db, get raw data (?), 
    /// "Tables API" - get table with filters, get table metadata, get entire table,
    /// "Useful Data and Lists" - (no longer available?)
    /// https://github.com/lppkarl/Quandl.NET




    let demo() =
        //let db = ("YAHOO", "MX_IBM")
        // --- FUTURES
        let db = ("WIKI", "FB")
        // --- CONTINUOUS
        //let db = ("CHRIS", "ICE_G1")            // GasOil
        //let db = ("CHRIS", "ICE_O1")            // Heating Oil
        //let db = ("CHRIS", "ICE_B1")            // Brent Crude
        // --- ZILLOW
        //let db = ("ZILLOW", "Z46321_IMP")       // Zillow: Munster
        // --- UMICH CONSUMER SENTIMENT
        //let db = ("UMICH", "SOC22")             // UMich Consumer Sentiment

        let result = GetTimeseries db
        let resultData = result.DatasetData.Data

    
        let columnNames = [ for column in result.DatasetData.ColumnNames do column + ";" ]
        printfn "COLUMN NAMES:\n%A" columnNames
    

        //processData parseUmichConsumerData "UMICH CONSUMER SURVEY DATA:" 100 resultData
        //processData parseZillowData "ZILLOW DATA:" 100 resultData
        //processData parseContinuousData "CONTINUOUS DATA:" 100 resultData   
        processData parseFuturesData "FUTURES DATA:" 100 resultData

    
        let item = resultData.Item (resultData.Count-1)
        printfn "\n\n%s %s    data count: %i" (fst db) (snd db) resultData.Count
        printfn "%A \n" item

















    (******************************** CODE GRAVEYARD *************************************)

    (*await client.Timeseries.GetDataAsync("WIKI", "FB");
    let dataTask = Async.AwaitTask (client.Timeseries.GetDataAsync("WIKI", "FB"))
    let result = dataTask.Result*)

    (* Output: "Date; Open; High; Low; Close; Volume; Ex-Dividend; Split Ratio; Adj. Open; Adj. High; Adj. Low; Adj. Close; Adj. Volume"
    printfn "%s" (String.Join ["; "; result.DatasetData.ColumnNames])
     Output: "2017-05-26; 152.23; 152.25; 151.15; 152.13; 14907827; 0; 1; 152.23; 152.25; 151.15; 152.13; 14907827"
    printfn "%s" (string.Join("; ", result.DatasetData.Data.First()))
    for (CsvFinancialFormat tick in data)
    {
        //Console.WriteLine(tick.Time.ToShortDateString() + " H: " + tick.High);
        Console.WriteLine(tick.InputString);
    }*)


    (*let processStocks displayCount data =
    printfn "FUTURES DATA:"
    let li = List.ofSeq data    // use List.ofSeq to convert 'List<T>' to 'T list'
    li                                              // print a list of rows of data
    |> List.take (Math.Min(displayCount, li.Length))
    //|> List.map (fun x -> printfn "%A" x; parseContinuousData x)
    |> List.map parseFuturesData
    |> List.iter (fun x -> printfn "%s" (x.ToString()))
    //|> List.iter (printfn "%A")
     
    let processContinuous displayCount data =
    printfn "CONTINUOUS DATA:"
    let li = List.ofSeq data            // use List.ofSeq to convert 'List<T>' to 'T list'
    li                                  // print a list of rows of data
    |> List.take (Math.Min(displayCount, li.Length))
    //|> List.map (fun x -> printfn "%A" x; parseContinuousData x)
    |> List.map parseContinuousData
    |> List.iter (fun x -> printfn "%s" (x.ToString()))
    //|> List.iter (printfn "%A")
     
    let processZillow displayCount data =
    printfn "ZILLOW DATA:"
    let li = List.ofSeq data
    li
    |> List.take (Math.Min(displayCount, li.Length))
    //|> List.map (fun x -> printfn "%A" x; parseContinuousData x)
    |> List.map parseZillowData
    |> List.iter (fun x -> printfn "%s" (x.ToString()))
    //|> List.iter (printfn "%A")

    let processUmichConsumer displayCount data =
    printfn "UMICH CONSUMER SURVEY DATA:"
    let li = List.ofSeq data
    li
    |> List.take (Math.Min(displayCount, li.Length))
    //|> List.map (fun x -> printfn "%A" x; parseContinuousData x)
    |> List.map parseUmichConsumerData
    |> List.iter (fun x -> printfn "%s" (x.ToString()))
    //|> List.iter (printfn "%A")*)


    (*processUmichConsumer 50 resultData
    processZillow 50 resultData
    processContinuous resultData
    processStocks resultData*)



    (*let dataTask = client.Timeseries.GetDataAsync(fst db, snd db)       //https://fsharpforfunandprofit.com/posts/concurrency-async-and-parallel/
    dataTask.Wait()
    let result = dataTask.Result
    let resultData = result.DatasetData.Data*)
    





