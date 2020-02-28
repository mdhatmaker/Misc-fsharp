module QuandlModule

/// https://github.com/lppkarl/Quandl.NET
/// https://fsharp.github.io/FSharp.Data/library/JsonValue.html

open System
open System.Net
open FSharp.Data
open Quandl.NET
open System.Collections.Generic

open QuandlTypes
open ChartModule


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

//[<StructuredFormatDisplay("{dt}   o:{os} h:{hs} l:{ls} c:{cs} v:{vs}   adjclose:{acs}")>]
[<StructuredFormatDisplay("{dt}  {os} {hs} {ls} {cs} {vs}  adjclose:{acs}")>]
type quote =
    { timestamp: int64
      o: float
      h: float
      l: float
      c: float
      v: float
      adjclose: float }
    member this.dtoffset = DateTimeOffset.FromUnixTimeSeconds(this.timestamp)  //val dt : DateTimeOffset = 2017/02/28 4:01:31 +00:00
    member this.utc = this.dtoffset.UtcDateTime
    member this.dt = this.dtoffset.LocalDateTime
    member this.os = this.o.ToString("0.00")
    member this.hs = this.h.ToString("0.00")
    member this.ls = this.l.ToString("0.00")
    member this.cs = this.c.ToString("0.00")
    member this.vs = this.v.ToString("0")
    member this.acs = this.adjclose.ToString("0.00")
    override m.ToString() = sprintf "%s  o:%.2f h:%.2f l:%.2f c:%.2f v:%.0f  ac:%.2f" (m.dt.ToString()) m.o m.h m.l m.c m.v m.adjclose


let tryIt() =
    
    
    (*let mutable urlTemplate = "http://ichart.finance.yahoo.com/table.csv?s=[symbol]&a=" +
                                "[startMonth]&b=[startDay]&c=[startYear]&d=[endMonth]&e=" +
                                "[endDay]&f=[endYear]&g=d&ignore=.csv"*)
    
    let urlTemplate = "https://query1.finance.yahoo.com/v7/finance/chart/YHOO" + 
                                "?symbol=[symbol]&range=2y&interval=1d&indicators=quote" +
                                "&includeTimestamps=true"
    
    let mutable values = Map.empty
    values <- values.Add("symbol", "GOOG")
    (*values <- values.Add("startMonth", "2")
    values <- values.Add("startDay", "1")
    values <- values.Add("startYear", "2018")
    values <- values.Add("endMonth", "3")
    values <- values.Add("endDay", "3")
    values <- values.Add("endYear", "2019")*)
    
    let mutable url = urlTemplate
    for kv in values do
        //printfn "%s <---> %s" kv.Key kv.Value
        let key = sprintf "[%s]" kv.Key
        url <- url.Replace(key, kv.Value)
    
    printfn "%s" url
    
    let downloadText (url: string) =
        let wc = new WebClient()
        let html = wc.DownloadString(Uri(url))
        html
    
    let jsonText = downloadText url

    let info = JsonValue.Parse(jsonText)
    
    //printfn "%A" info
    
    (*let prt dict =
        for kv in dict.Items do
            printfn "%s --> %A" kv.Key kv.Value*)

    let jchart = info.Item "chart"
    let jresult = jchart.Item "result"
    let jlist = jresult.[0]
    let jmeta = jlist.Item "meta"
    for k,v in jmeta.Properties() do
        printfn "META: %s %A" k v

    let jtimestamp = jlist.Item "timestamp"
    let jind = jlist.Item "indicators"
    let jquote' = jind.Item "quote"
    let jquote = jquote'.[0]
    let jopens = jquote.Item "open"
    let jhighs = jquote.Item "high"
    let jlows = jquote.Item "low"
    let jcloses = jquote.Item "close"
    let jvolumes = jquote.Item "volume"
    let jquotex = jind.Item "adjclose"
    let jadjcloses = jquotex.[0].Item "adjclose"
    let timestamps = jtimestamp.AsArray()
    let opens = jopens.AsArray()
    let highs = jhighs.AsArray()
    let lows = jlows.AsArray()
    let closes = jcloses.AsArray()
    let volumes = jvolumes.AsArray()
    let adjcloses = jadjcloses.AsArray()

    for i in [1..timestamps.Length-1] do
        let t = { timestamp=timestamps.[i].AsInteger64();
                  o=opens.[i].AsFloat();
                  h=highs.[i].AsFloat();
                  l=lows.[i].AsFloat();
                  c=closes.[i].AsFloat();
                  v=volumes.[i].AsFloat()
                  adjclose=adjcloses.[i].AsFloat()}
        printfn "%s" (t.ToString())
        //printfn "%A" t


    printfn "\n\n%A" (jind.Properties())

    printfn " "
    




    let quandlApiKey = "gCbpWopzuxctHw6y-qq5"
    let client = new QuandlClient(quandlApiKey)

    // Add the required settings to pull down data:
    let settings = new Dictionary<string, string>();
    settings.Add("collapse", "weekly");
    settings.Add("trim_start", "2010-02-01");
    settings.Add("trim_end", "2010-04-28");
    settings.Add("transformation", "normalize");
    settings.Add("sort_order", "asc");
    // Fetch:
    //IList<CsvFinancialFormat> data = myQuandl.GetData<CsvFinancialFormat>("YAHOO/MX_IBM", settings); //"GOOG/NYSE_IBM"
    //let data = myQuandl.GetData<CsvFinancialFormat>("YAHOO/MX_IBM", settings); //"GOOG/NYSE_IBM"


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


    let dataTask = client.Timeseries.GetDataAsync(fst db, snd db)       //https://fsharpforfunandprofit.com/posts/concurrency-async-and-parallel/
    dataTask.Wait()
    let result = dataTask.Result
    let resultData = result.DatasetData.Data

    let columnNames = [ for column in result.DatasetData.ColumnNames do column + ";" ]
    printfn "COLUMN NAMES:\n%A" columnNames
    

    (*// find a specific date
    resultData
    |>List.ofSeq 
    |> List.filter (fun x -> (string x.[0]) = "2018-03-19")
    |> List.map parseContinuousData
    |> printfn "%A"*)


    //processData parseUmichConsumerData "UMICH CONSUMER SURVEY DATA:" 100 resultData
    //processData parseZillowData "ZILLOW DATA:" 100 resultData
    //processData parseContinuousData "CONTINUOUS DATA:" 100 resultData
    processData parseFuturesData "FUTURES DATA:" 100 resultData

    
     

    let lastIndex = resultData.Count - 1
    let item = resultData.Item lastIndex
    printfn "\n\n%s %s    data count: %i" (fst db) (snd db) resultData.Count
    printfn "%A \n" item



    
    
    // https://query1.finance.yahoo.com/v7/finance/chart/YHOO?symbol=AAPL&range=2y&interval=1d&indicators=quote&includeTimestamps=true
    
    (*
    https://www.stock-data-solutions.com/kb/how-to-load-historical-prices-from-yahoo-finance-to-excel.htm
    
    daily, weekly, and monthly:
    https://query1.finance.yahoo.com/v7/finance/chart/YHOO?range=2y&interval=1d&indicators=quote&includeTimestamps=true
    https://query1.finance.yahoo.com/v7/finance/chart/YHOO?range=5y&interval=1wk&indicators=quote&includeTimestamps=true
    https://query1.finance.yahoo.com/v7/finance/chart/YHOO?range=max&interval=1mo&indicators=quote&includeTimestamps=true
    
    1, 5, 15, and 60-minute:
    https://query1.finance.yahoo.com/v7/finance/chart/YHOO?range=1d&interval=1m&indicators=quote&includeTimestamps=true
    https://query1.finance.yahoo.com/v7/finance/chart/YHOO?range=5d&interval=5m&indicators=quote&includeTimestamps=true
    https://query1.finance.yahoo.com/v7/finance/chart/YHOO?range=5d&interval=15m&indicators=quote&includeTimestamps=true
    https://query1.finance.yahoo.com/v7/finance/chart/YHOO?range=1mo&interval=60m&indicators=quote&includeTimestamps=true
    
    Use tickers like GBPUSD=X for currencies and like CK18.CBT for futures.
    *)
    










    (*let Bolivia = ["2004/05", 165.; "2005/06", 135.; "2006/07", 157.; "2007/08", 139.; "2008/09", 136.]
    let Ecuador = ["2004/05", 938.; "2005/06", 1120.; "2006/07", 1167.; "2007/08", 1110.; "2008/09", 691.]
    let Madagascar = ["2004/05", 522.; "2005/06", 599.; "2006/07", 587.; "2007/08", 615.; "2008/09", 629.]
    let Average = ["2004/05", 614.6; "2005/06", 682.; "2006/07", 623.; "2007/08", 609.4; "2008/09", 569.6]
       
    let series = [ "bars"; "bars"; "bars"; "lines" ]
    let inputs = [ Bolivia; Ecuador; Madagascar; Average ]
    let labels = ["Bolivia"; "Ecuador"; "Madagascar"; "Average"]

    let chart1 = createChart "Coffee Production" series labels inputs
    showChart chart1*)













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









