module YahooFinanceModule



open System
open System.Net
open FSharp.Data



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




