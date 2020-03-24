module MyLiveCharts

/// https://fslab.org/FSharp.Charting/LiveChartSamples.html
/// https://github.com/fslaborg/FSharp.Charting/blob/master/docs/content/PointAndLineCharts.fsx


#if INTERACTIVE
// On Mac OSX use FSharp.Charting.Gtk.fsx
#I "../packages/FSharp.Charting.2.1.0"
#load "FSharp.Charting.fsx"
#load "EventEx-0.1.fsx"
#r "../../packages/FSharp.Control.AsyncSeq.2.0.23/lib/net45/FSharp.Control.AsyncSeq.dll"
#endif


open System
open FSharp.Charting
open FSharp.Charting.ChartTypes
open FSharp.Control
open LiveChartEvent

/// Generate smaple data - both as a list of values and as an
/// F# event (IObservable) that can be passed to live charts:
let timeSeriesData = 
    [ for x in 0 .. 99 -> 
        DateTime.Now.AddDays (float x),sin(float x / 10.0) ]
let rnd = new System.Random()
let rand() = rnd.NextDouble()

let data = [ for x in 0 .. 99 -> (x,x*x) ]
let data2 = [ for x in 0 .. 99 -> (x,sin(float x / 10.0)) ]
let data3 = [ for x in 0 .. 99 -> (x,cos(float x / 10.0)) ]
let incData = Event.clock 10 |> Event.map (fun x -> (x, x.Millisecond))

let incBubbleData = 
    Event.clock 10 
    |> Event.map (fun x -> (rand(), rand(), rand()))

let evData = 
    Event.clock 10 
    |> Event.map (fun x -> (x, x.Millisecond)) 
    |> Event.windowTimeInterval 3000 

let evData2 = 
    Event.clock 20 
    |> Event.map (fun x -> (x, 100.0 + 200.0 * sin (float (x.Ticks / 2000000L)))) 
    |> Event.windowTimeInterval 3000 

let evBubbleData = 
    Event.clock 10 
    |> Event.map (fun x -> (rand(), rand(), rand())) 
    |> Event.sampled 30 
    |> Event.windowTimeInterval 3000  
    |> Event.map (Array.map (fun (_,x) -> x))

let constantLiveTimeSeriesData = Event.clock 30 |> Event.map (fun _ -> timeSeriesData)


/// You can use 'Event.cycle' to create a simple live data source that
/// iterates over in-memory sequence, or you can use an event as follows:
let liveDemo1 () =
    // Cycle through two data sets of the same type
    LiveChart.Line (Event.cycle 1000 [data2; data3])

let liveDemo2 () =
    LiveChart.LineIncremental(incData,Name="MouseMove") 
      .WithXAxis(Enabled=false).WithYAxis(Enabled=false)

let liveDemo3 () =
    LiveChart.FastLineIncremental(incData,Name="MouseMove")
      .WithXAxis(Enabled=false).WithYAxis(Enabled=false)

let liveDemo4 () =
    LiveChart.Line(evData,Name="MouseMove")
      .WithXAxis(Enabled=false).WithYAxis(Enabled=false)

let liveDemo5 () =
    LiveChart.Line(evData2,Name="Clock")
      .WithXAxis(Enabled=false).WithYAxis(Enabled=false)

let liveDemo6 () =
    LiveChart.Line(evData2,Name="Clock")
        .WithXAxis(Title="abc")
        .WithYAxis(Title="def")

let liveDemo7 () =
    (*LiveChart.Point(evData2,Name="Clock")
    LiveChart.FastPoint(evData2,Name="Clock")
    LiveChart.PointIncremental(incData,Name="Clock")
    LiveChart.FastPointIncremental(incData,Name="Clock")
    LiveChart.BubbleIncremental(incBubbleData,Name="Clock")*)
    LiveChart.Bubble(evBubbleData,Name="Clock")

let liveDemo8 () =
    LiveChart.Line(evData2,Name="Clock")
        .WithXAxis(Title="")
        .WithYAxis(Title="")

let liveDemo9 () =
    LiveChart.Line(evData2).WithTitle("A")

let liveDemo10 () =
    LiveChart.Line(evData,Name="MouseMove")

let liveDemo11 () =
    LiveChart.Line(constantLiveTimeSeriesData)

let liveDemo12 () =
    asyncSeq { 
        yield (1,10) 
        for i in 0 .. 100 do 
           do! Async.Sleep 100
           yield (i,i*i) 
        }
     |> AsyncSeq.toObservable
     |> LiveChart.LineIncremental


/// Returns: GenericChart
let demo (demoId:string) =
    //printfn "STARTING %s......\n" __SOURCE_FILE__
    match demoId with
    | "11" -> liveDemo1()
    | "12" -> liveDemo2()
    | "13" -> liveDemo3()
    | "14" -> liveDemo4()
    | "15" -> liveDemo5()
    | "16" -> liveDemo6()
    | "17" -> liveDemo7()
    | "18" -> liveDemo8()
    | "19" -> liveDemo9()
    | "20" -> liveDemo10()
    | "21" -> liveDemo11()
    | "22" -> liveDemo12()
    //printfn "\n...........%s END" __SOURCE_FILE__


