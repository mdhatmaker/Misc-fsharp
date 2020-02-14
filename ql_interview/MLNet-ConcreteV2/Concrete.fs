
module Concrete

// http://www.codesuji.com/2019/09/14/F-and-MLNet-Regression-V2/

open Microsoft.ML
open Microsoft.ML.Data

(* The ConcreteData record is effectively a file definition.
ML.NET requires the LoadColumn attribute for column mappings used during the
dataload phase.
The ConcretePrediction record is for prediction results.
Once the datatypes are setup, an MLContext object must be created. *)

[<CLIMutable>]
type ConcreteData = {
    [<LoadColumn(0)>]
    Cement: float32

    [<LoadColumn(1)>]
    Slag: float32

    [<LoadColumn(2)>]
    Ash: float32

    [<LoadColumn(3)>]
    Water: float32

    [<LoadColumn(4)>]
    Superplasticizer: float32

    [<LoadColumn(5)>]
    CoarseAggregate: float32

    [<LoadColumn(6)>]
    FineAggregate: float32

    [<LoadColumn(7)>]
    Age: float32

    [<LoadColumn(8)>]
    Label: float32
    }

[<CLIMutable>]
type ConcretePrediction = {
    Score: float32
    }

// We pull one row from the data for use in performing a prediction test. *)
let test1 = {
    Cement = 198.6f
    Slag = 132.4f
    Ash = 0.f
    Water = 192.f
    Superplasticizer = 0.f
    CoarseAggregate = 978.4f
    FineAggregate = 825.5f
    Age = 90.f
    Label = 0.f
    }

let testLoadModel() =
    let context = MLContext()

    // Load model from file
    let (modelReloaded, schemaReloaded) = context.Model.Load("model.zip")
    let predictionEngineReloaded = context.Model.CreatePredictionEngine<ConcreteData,ConcretePrediction>(modelReloaded)
    let predictionReloaded = predictionEngineReloaded.Predict(test1)

    printfn "Sample prediction, but on a model loaded from a file:"
    printfn "Predicted Strength: %f" predictionReloaded.Score
    printfn "Actual Strength   : 38.074243671999994"
    printfn ""


// Set doSaveModel to true if you want to save the model to a file.
let doSaveModel = false

let main() =
    let context = MLContext()

    let dataPath = "./data/Concrete_Data.csv"
    let allData =
        context
            .Data
            .LoadFromTextFile<ConcreteData>(
                path = dataPath,
                hasHeader = true,
                separatorChar = ',')

    let filteredData = context.Data.FilterRowsByColumn(allData, "Slag", lowerBound = 50., upperBound = 100.)

    let allDataSplit = context.Data.TrainTestSplit(allData, testFraction =0.1)
    let trainData = allDataSplit.TrainSet
    let testData = allDataSplit.TestSet

    // Possible normalization methods: NormalizeLogMeanVariance, NormalizeLpNorm, NormalizeMinMax

    (* The data is already in numeric form, but if there were text fields, 
    there are transformation methods for that as well. An example of this could be:
    Transforms.Categorical.OneHotEncoding("CementBrandName", "CementBrandId") *)

    // Concatenate provides a mechanism to build an aggregate field, specifically Features.

    let pipeline =
        EstimatorChain()
            .Append(context.Transforms.NormalizeMeanVariance("Cement", "Cement"))
            .Append(context.Transforms.NormalizeMeanVariance("Slag", "Slag"))
            .Append(context.Transforms.NormalizeMeanVariance("Ash", "Ash"))
            .Append(context.Transforms.NormalizeMeanVariance("Water", "Water"))
            .Append(context.Transforms.NormalizeMeanVariance("Superplasticizer", "Superplasticizer"))
            .Append(context.Transforms.NormalizeMeanVariance("CoarseAggregate", "CoarseAggregate"))
            .Append(context.Transforms.NormalizeMeanVariance("FineAggregate", "FineAggregate"))
            .Append(context.Transforms.NormalizeMeanVariance("Age", "Age"))
            .Append(context.Transforms.Concatenate("Features", [|"Cement"; "Slag"; "Ash"; "Water"; "Superplasticizer"; "CoarseAggregate"; "FineAggregate"; "Age"|]))

    (* Once the data pipeline is configured, it is time to build a model trainer.
    Again, ML.NET offers multiple options for training methods. For this example,
    we use FastTreeTweedie with no parameters. Hyperparameter options are available
    for many of the trainers. We show some additional examples of how to
    implement alternative trainers with and without hyperparameters. Once the
    trainer is defined, it is appended to the pipeline. Now is the time to create
    a trained model using Fit against the previously defined training data.
    There is one last piece to make this process useful, the prediction engine.
    This provides the mechanism to actually perform predictions. *)

    let trainer = context.Regression.Trainers.FastTreeTweedie()

    // FastTreeRegressor with hyperparameters
    let trainer = context.Regression.Trainers.FastTreeTweedie(numberOfTrees = 500, minimumExampleCountPerLeaf = 5)

    // OnlineGradientDescent
    let trainer = context.Regression.Trainers.OnlineGradientDescent(labelColumnName = "Label", featureColumnName = "Features")

    let modelBuilder = pipeline.Append(trainer)

    let model = modelBuilder.Fit(trainData)

    let predictionEngine = context.Model.CreatePredictionEngine<ConcreteData,ConcretePrediction>(model)

    (* The next step is to see how good of a model has been built. The trained
    model is now applied to the test data, and performance metrics are extracted. *)

    let predictions = model.Transform(testData)

    let metrics = context.Regression.Evaluate(predictions)

    printfn "R-Squared: %f" (metrics.RSquared)
    printfn "RMS      : %f" (metrics.RootMeanSquaredError)
    printfn "Loss     : %f" (metrics.LossFunction)
    printfn "MAE      : %f" (metrics.MeanAbsoluteError)
    printfn "MSE      : %f" (metrics.MeanSquaredError)
    printfn ""

    (* Sample evaluation metrics for test data run:
    R-Squared: 0.920959
    RMS      : 4.875440
    Loss     : 23.769913
    MAE      : 2.682631
    MSE      : 23.769913 *)

    (* The trained model is now something that can be used against data.
    We can pull one row from the data just to show how this is put together.
    Again, we use a record type to define the data. *)

    (*let test1 = {
        Cement = 198.6f
        Slag = 132.4f
        Ash = 0.f
        Water = 192.f
        Superplasticizer = 0.f
        CoarseAggregate = 978.4f
        FineAggregate = 825.5f
        Age = 90.f
        Label = 0.f
        }*)

    let predictionTest1 = predictionEngine.Predict(test1)
    printfn "Sample prediction:"
    printfn "Predicted Strength: %f" predictionTest1.Score
    printfn "Actual Strength   : 38.074243671999994"
    printfn ""


    (* A trained model isn't much use if it can't be passed around and used 
    elsewhere. That is where the ML.NET model save and load methods come into
    play. They are both straightforward to use. The same prediction as above
    is run, but this time on a model loaded from a file. *)

    if (doSaveModel) then
        // Save model to file
        context.Model.Save(model, trainData.Schema, "model.zip")




