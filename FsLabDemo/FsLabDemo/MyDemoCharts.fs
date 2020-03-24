module MyDemoCharts

// For more about F#, see:
//     https://fsharp.org
//     https://docs.microsoft.com/en-us/dotnet/articles/fsharp/
//
// To see the tutorial in documentation form, see:
//     https://docs.microsoft.com/en-us/dotnet/articles/fsharp/tour
//
// To learn more about applied F# programming, use
//     https://fsharp.org/guides/enterprise/
//     https://fsharp.org/guides/cloud/
//     https://fsharp.org/guides/web/
//     https://fsharp.org/guides/data-science/


/// https://fslab.org/FSharp.Charting/
/// https://fslab.org/FSharp.Charting/ReferencingTheLibrary.html
/// https://fslab.org/FSharp.Charting/StockAndCandlestickCharts.html


#if INTERACTIVE
#load "../packages/FsLab.1.1.6/FsLab.fsx"
// On Mac OSX use packages/FSharp.Charting.Gtk.2.0.0/FSharp.Charting.Gtk.fsx
//#l "../packages/FSharp.Charting.2.1.0/FSharp.Charting.fsx"
#l "../packages/FSharp.Charting.2.1.0"
#load "../packages/FSharp.Charting.2.1.0/FSharp.Charting.fsx"
#r "../packages/FSharp.Data.2.4.6/lib/net45/FSharp.Data.dll"
#r "../packages/FSharp.Charting.2.1.0/lib/net45/FSharp.Charting.dll"
#endif


//open FsLab
open System
open FSharp.Data
open FSharp.Charting
open System.Windows.Forms.DataVisualization

/// A stock or a candlestick chart can be created using the FSharpChart.Stock and
/// FSharpChart.Candlestick methods. Financial charts for visualizing stocks require
/// four values for drawing each data point (High, Low, Open, and Close price). When
/// calling the methods, it is possible to specify the values as a collection containing
/// four-element tuples, or five-element tuples (Date, High, Low, Open, and Close price).

let prices =
  [ 26.24,25.80,26.22,25.95; 26.40,26.18,26.26,26.20
    26.37,26.04,26.11,26.08; 26.78,26.15,26.60,26.16
    26.86,26.51,26.69,26.58; 26.95,26.50,26.91,26.55
    27.06,26.50,26.64,26.77; 26.86,26.43,26.53,26.59
    27.10,26.52,26.78,26.59; 27.21,26.99,27.13,27.06
    27.37,26.91,26.97,27.21; 27.07,26.60,27.05,27.02
    27.33,26.95,27.04,26.96; 27.27,26.95,27.21,27.23
    27.81,27.07,27.76,27.25; 27.94,27.29,27.93,27.50
    28.26,27.91,28.19,27.97; 28.34,28.05,28.10,28.28
    28.34,27.79,27.80,28.20; 27.84,27.51,27.70,27.77 ]


/// When using F# Interactive, each of these examples needs to be evaluated separately.
/// This way, F# Interactive invokes a handler that automatically shows the created chart.

#if INTERACTIVE
Chart.Line [ for x in 0 .. 10 -> x, x*x ]
Chart.Candlestick(prices).WithYAxis(Max = 29.0, Min = 25.0)
Chart.Stock(prices).WithYAxis(Max = 29.0, Min = 25.0)
#endif

//Chart.Candlestick(prices).WithYAxis(Max = 29.0, Min = 25.0).ShowChart()
//Chart.Stock(prices).WithYAxis(Max = 29.0, Min = 25.0).ShowChart()

/// Alternatively, you can reference FSharp.Charting.dll directly and manually display
/// the charts using ShowChart:

//Chart.Line([ for x in 0 .. 10 -> x, x*x ]).ShowChart()


//Console.ReadKey() //|> ignore


let rnd = new Random()
let rand() = rnd.NextDouble()
let randomPoints = [ for i in 0 .. 1000 -> rand(), rand() ]


let demo4 () =
    // Draw scatter plot  of points
    //randomPoints |> Chart.Point(Title = "Scatter Plot") |> Chart.Show
    // TODO: How do I pass 'Title' to the above "pipelined" version?
    Chart.Point(data=randomPoints, Name="Scatter Plot").ShowChart() |> ignore

let demo5 () =
    let pricesWithDates = 
        prices |> List.mapi (fun i (hi,lo,op,cl) -> 
            (DateTime.Today.AddDays(float i).ToShortDateString(), hi, lo, op, cl))
    // Candlestick chart price range specified
    Chart.Candlestick(pricesWithDates).WithYAxis(Max = 29.0, Min = 25.0).ShowChart() |> ignore
    
(*let demo5'() =
    let pricesWithDates = 
        prices |> List.mapi (fun i (hi,lo,op,cl) -> 
            (DateTime.Today.AddDays(float i).ToShortDateString(), hi, lo, op, cl))
    // Alternative specification using pipelining
    Chart.Candlestick(pricesWithDates)
    |> Chart.WithYAxis(Max = 29.0, Min = 25.0)*)


let demo6 () =
    let futureDate numDays = DateTime.Today.AddDays(float numDays)

    let expectedIncome = 
      [ for x in 1 .. 100 -> 
          futureDate x, 1000.0 + rand() * 100.0 * exp (float x / 40.0) ]
    let expectedExpenses = 
      [ for x in 1 .. 100 -> 
          futureDate x, rand() * 500.0 * sin (float x / 50.0) ]
    let computedProfit = 
      (expectedIncome, expectedExpenses) 
      ||> List.map2 (fun (d1,i) (d2,e) -> (d1, i - e))
    
    Chart.Combine(
       [ Chart.Line(expectedIncome,Name="Income")
         Chart.Line(expectedExpenses,Name="Expenses") 
         Chart.Line(computedProfit,Name="Profit") ]).ShowChart() |> ignore
    ()  // return unit

let demo7 () =
    let data = WorldBankData.GetDataContext()
    data
        .Countries.``United Kingdom``
        .Indicators.``Gross capital formation (% of GDP)``
    |> Chart.Line |> Chart.Show     // TODO: How do I pass 'Title' to Chart.Line?
    let revenue = data.Countries.Africa.Indicators.``Revenue, excluding grants (% of GDP)``
    let expense = data.Countries.Africa.Indicators.``Expense (% of GDP)``
    let subTuple (t1:int*float) (t2:int*float) =
        let t1a,t1b = t1
        let t2a,t2b = t2
        // assert: t1a = t2a
        if t1a <> t2a then
            printfn "ERROR! subTuple: 1st tuple elements should be equal"
        (t1a, (t1b-t2b))

    let net = Seq.zip revenue expense
              |> Seq.map (fun (tup1,tup2) -> subTuple tup1 tup2 )
              |> printfn "%A"
    ()  // return unit

// If the 'Asynchronous' parameter is set to 'true' then the type provider
// generates all operations as asynchronous:
type WorldBank = WorldBankDataProvider<"World Development Indicators", Asynchronous=true>

/// Downloading data in parallel
let demo8 () =
    let wb = WorldBank.GetDataContext()
    // Create a list of countries to process
    let countries =
        [| wb.Countries.``Arab World``
           wb.Countries.``European Union``
           wb.Countries.Australia
           wb.Countries.Brazil
           wb.Countries.Canada
           wb.Countries.Chile
           wb.Countries.``Czech Republic``
           wb.Countries.Denmark
           wb.Countries.France
           wb.Countries.Greece
           wb.Countries.``Low income``
           wb.Countries.``High income``
           wb.Countries.``United Kingdom``
           wb.Countries.``United States`` |]
    
    // To download the information in parallel, we can create a list of asynchronous
    // computations, compose them using 'Async.Parallel' and then run the (single)
    // obtained computation to perform all the downloads:
    [ for c in countries ->
        c.Indicators.``Gross capital formation (% of GDP)`` ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> Array.map Chart.Line
    |> Chart.Combine
    |> Chart.Show |> ignore
    // The above snippet downloads the data using 'Async.RunSynchronously', but
    // it also turns every single downloaded data set into a line chart (using
    // 'Chart.Line') and then creates a single composed chart using 'Chart.Combine'.
    ()  // return unit



/// Returns: unit
let demo (demoId:string) =
    printfn "STARTING %s......\n" __SOURCE_FILE__

    match demoId with
    | "1" -> Chart.Candlestick(prices).WithYAxis(Max = 29.0, Min = 25.0).ShowChart() |> ignore
    | "2" -> Chart.Stock(prices).WithYAxis(Max = 29.0, Min = 25.0).ShowChart() |> ignore
    | "3" -> Chart.Line([ for x in 0 .. 10 -> x, x*x ]).ShowChart() |> ignore
    | "4" -> demo4()
    | "5" -> demo5()
    | "6" -> demo6()
    | "7" -> demo7()
    | "8" -> demo8()
    | _ -> printfn "ERROR! MyDemoCharts.demo: No match for demoId '%s'" demoId

    printfn "\n...........%s END" __SOURCE_FILE__

        



/// https://github.com/fslaborg/FSharp.Charting/blob/master/docs/content/UsingChartsInWPF.md

// Using Charts in WPF applications
//
// To host a chart object inside a WPF application,
// - Add a reference to the FSharp.Charting nuget package
// - Add references to the following additional DLLs from the base class library:
//   * System.Drawing.dll
//   * System.Windows.Forms.dll
//   * WindowsFormsIntegration.dll
// - Add a WindowsFormHost element to your XAML in your F# application and
//   give the element a name: <WindowsFormsHost x:Name="WinForm" />
// - In your application code, set the Child property of the host to a
//   chart control created from a chart specification:
//
//   open FSharp.Charting
//   open FSharp.Charting.ChartTypes
//   // ...
//
//   let winForm = window.Root.FindName("WinForm") :?> WindowsFormsHost
//
//   let chart = Chart.Line [ for i in 0 .. 10 -> (i,i*i) ]
//
//   winForm.Child <- new ChartControl(chart)
//
// - Live and Incremental updating charts can also be used in this way:
//  
//   let dataStream =
//       window.Root.MouseMove
//       |> Event.map (fun x -> let p = x.GetPosition(window.Root) in p.X,p.Y )
//   let chart = LiveChart.LineIncremental (dataStream)
//   
//   winForm.Child <- new ChartControl(chart)
//


