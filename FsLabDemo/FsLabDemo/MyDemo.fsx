module MyDemo

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


let demo4() =
    let pricesWithDates = 
        prices |> List.mapi (fun i (hi,lo,op,cl) -> 
            (DateTime.Today.AddDays(float i).ToShortDateString(), hi, lo, op, cl))
    // Candlestick chart price range specified
    Chart.Candlestick(pricesWithDates).WithYAxis(Max = 29.0, Min = 25.0).ShowChart() |> ignore
    
let demo4'() =
    let pricesWithDates = 
        prices |> List.mapi (fun i (hi,lo,op,cl) -> 
            (DateTime.Today.AddDays(float i).ToShortDateString(), hi, lo, op, cl))
    // Alternative specification using pipelining
    Chart.Candlestick(pricesWithDates)
    |> Chart.WithYAxis(Max = 29.0, Min = 25.0)
    |> Chart.Show




let demo (ndemo:int) =
    printfn "STARTING %s......\n" __SOURCE_FILE__

    (*let prices =
      [ 26.24,25.80,26.22,25.95; 26.40,26.18,26.26,26.20
        26.37,26.04,26.11,26.08; 26.78,26.15,26.60,26.16
        26.86,26.51,26.69,26.58; 26.95,26.50,26.91,26.55
        27.06,26.50,26.64,26.77; 26.86,26.43,26.53,26.59
        27.10,26.52,26.78,26.59; 27.21,26.99,27.13,27.06
        27.37,26.91,26.97,27.21; 27.07,26.60,27.05,27.02
        27.33,26.95,27.04,26.96; 27.27,26.95,27.21,27.23
        27.81,27.07,27.76,27.25; 27.94,27.29,27.93,27.50
        28.26,27.91,28.19,27.97; 28.34,28.05,28.10,28.28
        28.34,27.79,27.80,28.20; 27.84,27.51,27.70,27.77 ]*)

    match ndemo with
    | 1 -> Chart.Candlestick(prices).WithYAxis(Max = 29.0, Min = 25.0).ShowChart() |> ignore
    | 2 -> Chart.Stock(prices).WithYAxis(Max = 29.0, Min = 25.0).ShowChart() |> ignore
    | 3 -> Chart.Line([ for x in 0 .. 10 -> x, x*x ]).ShowChart() |> ignore
    | 4 -> demo4()




    printfn "\n...........%s END" __SOURCE_FILE__

        




