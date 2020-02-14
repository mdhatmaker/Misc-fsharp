module PriceAnomalies

open Microsoft.ML
open Microsoft.ML.Data
open Microsoft.ML.Transforms.TimeSeries
open XPlot.GoogleCharts


(* PriceData matches the datafile schema. PricePrediction is for the model
results, in this case we’ll use it for both anomaly detection and change point
detection results. The Prediction field is an array containing a 0 or 1 for a
detected event, the value at that datapoint, and its respective confidence level. *)

type PriceData () =
    [<DefaultValue>]
    [<LoadColumn(0)>]
    val mutable public Date:string

    [<DefaultValue>]
    [<LoadColumn(1)>]
    val mutable public Open:float32

    [<DefaultValue>]
    [<LoadColumn(2)>]
    val mutable public High:float32

    [<DefaultValue>]
    [<LoadColumn(3)>]
    val mutable public Low:float32

    [<DefaultValue>]
    [<LoadColumn(4)>]
    val mutable public Close:float32

    [<DefaultValue>]
    [<LoadColumn(5)>]
    val mutable public AdjClose:float32

    [<DefaultValue>]
    [<LoadColumn(6)>]
    val mutable public Volume:float32

type PricePrediction () =
    [<DefaultValue>]
    val mutable public Date:string

    [<DefaultValue>]
    val mutable public Prediction:double[]


let main() =
    (* Time for the processing pipeline. This includes creating the pipeline context
    and hooking up the data to the file. 
    To process the data, there will technically be two pipelines:
    1) use the IidSpike trainer for anomaly detection
    2) use the IidChangePoint trainer for change point detection
    There are a couple dials to adjust to get best results:
    The pvalueHistoryLength defines the sliding window size that is applied when looking
    for events. Since this is stock data, multiples of 5 roughly correlate to weeks. So
    at anomalies over 6 week windows, and change points over 2 week windows.
    Additionally, confidence is on a scale 0-100, higher values requiring a higher level
    of confidence to trigger an event.
    Another dial to turn is AnomalySide to detect either only positive, only negative,
    or all anomalies (the default). *)

    // Export Dow Jones index data from Yahoo! Finance:
    // https://finance.yahoo.com/quote/%5EDJI?p=^DJI&.tsrc=fin-srch

    let dataPath = "./data/dji.csv"

    let ctx = MLContext()

    let dataView =
        ctx
            .Data
            .LoadFromTextFile<PriceData>(
                path = dataPath,
                hasHeader = true,
                separatorChar = ',')
            
    let anomalyPValueHistoryLength = 15 //30
    let changePointPValueHistoryLength = 5  //10
    let anomalyConfidence = 95
    let changePointConfidence = 95

    let anomalyPipeline =
        ctx
            .Transforms
            .DetectIidSpike(
                outputColumnName = "Prediction",
                inputColumnName = "Close",
                side = AnomalySide.TwoSided,
                confidence = anomalyConfidence,
                pvalueHistoryLength = anomalyPValueHistoryLength)

    let changePointPipeLine =
        ctx
            .Transforms
            .DetectIidChangePoint(
                outputColumnName = "Prediction",
                inputColumnName = "Close",
                martingale = MartingaleType.Power,
                confidence = changePointConfidence,
                changeHistoryLength = changePointPValueHistoryLength)

    let trainedAnomalyModel = anomalyPipeline.Fit(dataView)
    let trainedChangePointModel = changePointPipeLine.Fit(dataView)

    let transformedAnomalyData = trainedAnomalyModel.Transform(dataView)
    let transformedChangePointData = trainedChangePointModel.Transform(dataView)

    let anomalies =
        ctx
            .Data
            .CreateEnumerable<PricePrediction>(transformedAnomalyData, reuseRowObject = false)

    let changePoints =
        ctx
            .Data
            .CreateEnumerable<PricePrediction>(transformedChangePointData, reuseRowObject = false)


    (* Now build some charts and look at the results. *)

    // Build chart data
    let priceChartData =
        anomalies
        |> Seq.map (fun p -> let p' = float (p.Prediction).[1] in (p.Date, p'))
        |> List.ofSeq

    let anomalyChartData =
        anomalies
        |> Seq.map (fun p -> let p' = if (p.Prediction).[0] = 0. then None else Some (float (p.Prediction).[1]) in (p.Date, p'))
        |> Seq.filter (fun (x,y) -> y.IsSome)
        |> Seq.map (fun (x,y) -> (x, y.Value))
        |> List.ofSeq

    let changePointChartData =
        changePoints
        |> Seq.map (fun p -> let p' = if (p.Prediction).[0] = 0. then None else Some (float (p.Prediction).[1]) in (p.Date, p'))
        |> Seq.filter (fun (x,y) -> y.IsSome)
        |> Seq.map (fun (x,y) -> (x, y.Value))
        |> List.ofSeq

    // Show Chart
    [priceChartData; anomalyChartData; changePointChartData]
    |> Chart.Combo
    |> Chart.WithOptions
        (Options(title = "Dow Jones Industrial Average Price Anomalies",
                series = [| Series("lines"); Series("scatter"); Series("scatter") |],
                displayAnnotations = true))
    |> Chart.WithLabels ["Price"; "Anomaly"; "ChangePoint" ]
    |> Chart.WithLegend true
    |> Chart.WithSize (1200, 800) //(800, 400)
    |> Chart.Show


      


