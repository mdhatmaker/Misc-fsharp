// http://www.codesuji.com/2019/01/14/F-and-MLNet-Sentiment-Analysis/

open System
open Microsoft.ML
open Microsoft.ML.Data


[<CLIMutable>]
type SentimentData = {
    [<LoadColumn(0)>]
    SentimentText: string

    [<LoadColumn(1)>]
    Label: bool

    [<LoadColumn(2)>]
    Probability: float32
}

[<CLIMutable>]
type SentimentPrediction = {
    SentimentData: string
    PredictedLabel: bool
    Score: float32
}






[<EntryPoint>]
let main argv =
    printfn "Sentiment Analysis with ML.NET and F#\n"

    let ctx = MLContext()

    let dataPath = "./data/imdb_labelled.txt"

    let allData = ctx.Data.LoadFromTextFile<SentimentData>(
        path = dataPath, 
        separatorChar = '\t',
        hasHeader = true)

    //let allData = reader.Read(dataFile)
    //let struct (trainData, testData) = ml.Clustering.TrainTestSplit(allData, testFraction = 0.3)
    let allDataSplit = ctx.Data.TrainTestSplit(allData, testFraction =0.1)
    let trainData = allDataSplit.TrainSet
    let testData = allDataSplit.TestSet

    printfn "### Schema"
    allData.Schema
    |> Seq.iter (fun x -> printfn "%A" x)
    printfn ""

    let pipeline = 
        ctx
            .Transforms.Text.FeaturizeText("Features", "SentimentText")
            //.Append(ml.BinaryClassification.Trainers.FastForest(numTrees=500, numLeaves=100, learningRate=0.0001))
            // Example of custom hyperparameters
            // .Append(mlContext.BinaryClassification.Trainers.FastForest())
    //let model = pipeline.Fit(trainData)

    //let trainer = ctx.Regression.Trainers.OnlineGradientDescent(labelColumnName = "Label", featureColumnName = "Features")
    //let trainer = ctx.Regression.Trainers.FastForest()
    let trainer = ctx.Regression.Trainers.FastTreeTweedie()

    let modelBuilder = pipeline.Append(trainer)
    let model = modelBuilder.Fit(trainData)
    let predictionEngine = ctx.Model.CreatePredictionEngine<SentimentData,SentimentPrediction>(model)


    (*
    //Step 1: Create a ML Context
    let ctx = MLContext()

    //Step 2: Read in the input data for model training 
    let dataReader = ctx.Data
        .LoadFromTextFile<MyInput>(dataPath, hasHeader = true)

    //Step 3: Build your estimator 
    let est = ctx.Transforms.Text
        .FeaturizeText("Features", "Text")
        .Append(ctx.BinaryClassification.Trainers
            .LbfgsLogisticRegression("Label", "Features"));

    //Step 4: Train your model    
    let trainedModel = est.Fit(trainingDataView)

    //Step 5: Make predictions using your model
    let predictionEngine = ctx.Model
        .CreatePredictionEngine<MyInput, MyOutput>(trainedModel)

    let sampleStatement = { Label = false; Text = "This is a horrible movie!" }

    let prediction = predictionEngine.Predict(sampleStatement)
    Built for .NET developers
    With ML.NET, you can create custom ML models using C# or F# without having to leave the .NET ecosystem.

    ML.NET lets you re-use all the knowledge, skills, code, and libraries you already have as a .NET developer so that you can easily integrate machine learning into your web, mobile, desktop, gaming, and IoT apps.
    *)


    let displayEvaluation description data =
        let predictions = model.Transform data

        let metrics = ctx.BinaryClassification.Evaluate(predictions)

        printfn ""
        printfn "### %s" description
        printfn "Accuracy          : %0.4f" (metrics.Accuracy)
        printfn "F1                : %0.4f" (metrics.F1Score)
        printfn "Positive Precision: %0.4f" (metrics.PositivePrecision)
        printfn "Positive Recall   : %0.4f" (metrics.PositiveRecall)
        printfn "Negative Precision: %0.4f" (metrics.NegativePrecision)
        printfn "Netative Recall   : %0.4f" (metrics.NegativeRecall)
        printfn ""

        let preview = predictions.Preview()
        preview.RowView
        |> Seq.take 5
        |> Seq.iter (fun row ->
            row.Values
            |> Array.iter (fun kv -> printfn "%s: %A" kv.Key kv.Value)
            printfn "")
        printfn ""

    displayEvaluation "Train" trainData
    displayEvaluation "Test" testData


    0 // return an integer exit code
