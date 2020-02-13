
module Housing

// https://archive.ics.uci.edu/ml/machine-learning-databases/housing/
// http://www.codesuji.com/2019/09/14/F-and-MLNet-Regression-V2/

open Microsoft.ML
open Microsoft.ML.Data

(* The HousingData record is effectively a file definition.
ML.NET requires the LoadColumn attribute for column mappings used during the
dataload phase.
The HousingPrediction record is for prediction results.
Once the datatypes are setup, an MLContext object must be created. *)

(* Attribute Information:
    1. CRIM      per capita crime rate by town
    2. ZN        proportion of residential land zoned for lots over 
                 25,000 sq.ft.
    3. INDUS     proportion of non-retail business acres per town
    4. CHAS      Charles River dummy variable (= 1 if tract bounds 
                 river; 0 otherwise)
    5. NOX       nitric oxides concentration (parts per 10 million)
    6. RM        average number of rooms per dwelling
    7. AGE       proportion of owner-occupied units built prior to 1940
    8. DIS       weighted distances to five Boston employment centres
    9. RAD       index of accessibility to radial highways
    10. TAX      full-value property-tax rate per $10,000
    11. PTRATIO  pupil-teacher ratio by town
    12. B        1000(Bk - 0.63)^2 where Bk is the proportion of blacks 
                 by town
    13. LSTAT    % lower status of the population
    14. MEDV     Median value of owner-occupied homes in $1000's *)

[<CLIMutable>]
type HousingData = {
    [<LoadColumn(0)>]
    CRIM: float32

    [<LoadColumn(1)>]
    ZN: float32

    [<LoadColumn(2)>]
    INDUS: float32

    [<LoadColumn(3)>]
    CHAS: float32

    [<LoadColumn(4)>]
    NOX: float32

    [<LoadColumn(5)>]
    RM: float32

    [<LoadColumn(6)>]
    AGE: float32

    [<LoadColumn(7)>]
    DIS: float32

    [<LoadColumn(8)>]
    RAD: float32

    [<LoadColumn(9)>]
    TAX: float32

    [<LoadColumn(10)>]
    PTRATIO: float32

    [<LoadColumn(11)>]
    B: float32

    [<LoadColumn(12)>]
    LSTAT: float32

    [<LoadColumn(13)>]
    Label: float32
    }

[<CLIMutable>]
type HousingPrediction = {
    MedianValue: float32
    }


// We pull one row from the data for use in performing a prediction test. *)
let test1 = {
    //1.38799   0.00   8.140  0  0.5380  5.9500  82.00  3.9900  4  307.0  21.00 232.60  27.71  13.20
    CRIM = 1.38799f
    ZN = 0.f
    INDUS = 8.14f
    CHAS = 0.f
    NOX = 0.538f
    RM = 5.95f
    AGE = 82.f
    DIS = 3.99f
    RAD = 4.f
    TAX = 307.f
    PTRATIO = 21.f
    B = 232.6f
    LSTAT = 27.71f
    Label = 0.f
    }

let testLoadModel() =
    let context = MLContext()

    // Load model from file
    let (modelReloaded, schemaReloaded) = context.Model.Load("model_housing.zip")
    let predictionEngineReloaded = context.Model.CreatePredictionEngine<HousingData,HousingPrediction>(modelReloaded)
    let predictionReloaded = predictionEngineReloaded.Predict(test1)

    printfn "Sample prediction, but on a model loaded from a file:"
    printfn "Predicted Value: %f" predictionReloaded.MedianValue
    printfn "Actual Value   : 13.20"
    printfn ""


//let dataPath = "./data/Housing_Data.csv"
let dataPath = "./data/Housing_Data.txt"

//let convertDataFile() =


// Set doSaveModel to true if you want to save the model to a file.
let doSaveModel = false

let main() =
    let context = MLContext()

    let allData =
        context
            .Data
            .LoadFromTextFile<HousingData>(
                path = dataPath,
                hasHeader = false,
                separatorChar = ' ')

    
    //let filteredData = context.Data.FilterRowsByColumn(allData, "Slag", lowerBound = 50., upperBound = 100.)

    let allDataSplit = context.Data.TrainTestSplit(allData, testFraction = 0.1)
    let trainData = allDataSplit.TrainSet
    let testData = allDataSplit.TestSet

    // Possible normalization methods: NormalizeLogMeanVariance, NormalizeLpNorm, NormalizeMinMax

    (* The data is already in numeric form, but if there were text fields, 
    there are transformation methods for that as well. An example of this could be:
    Transforms.Categorical.OneHotEncoding("CementBrandName", "CementBrandId") *)

    // Concatenate provides a mechanism to build an aggregate field, specifically Features.

    let pipeline =
        EstimatorChain()
            .Append(context.Transforms.NormalizeMeanVariance("CRIM", "CRIM"))
            .Append(context.Transforms.NormalizeMeanVariance("ZN", "ZN"))
            .Append(context.Transforms.NormalizeMeanVariance("INDUS", "INDUS"))
            .Append(context.Transforms.NormalizeMeanVariance("CHAS", "CHAS"))
            .Append(context.Transforms.NormalizeMeanVariance("NOX", "NOX"))
            .Append(context.Transforms.NormalizeMeanVariance("RM", "RM"))
            .Append(context.Transforms.NormalizeMeanVariance("AGE", "AGE"))
            .Append(context.Transforms.NormalizeMeanVariance("DIS", "DIS"))
            .Append(context.Transforms.NormalizeMeanVariance("RAD", "RAD"))
            .Append(context.Transforms.NormalizeMeanVariance("TAX", "TAX"))
            .Append(context.Transforms.NormalizeMeanVariance("PTRATIO", "PTRATIO"))
            .Append(context.Transforms.NormalizeMeanVariance("B", "B"))
            .Append(context.Transforms.NormalizeMeanVariance("LSTAT", "LSTAT"))
            .Append(context.Transforms.Concatenate("Features", [|"CRIM"; "ZN"; "INDUS"; "CHAS"; "NOX"; "RM"; "AGE"; "DIS"; "RAD"; "TAX"; "PTRATIO"; "B"; "LSTAT"|]))

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

    (*
    // FastTreeRegressor with hyperparameters
    let trainer = context.Regression.Trainers.FastTreeTweedie(numberOfTrees = 500, minimumExampleCountPerLeaf = 5)

    // OnlineGradientDescent
    let trainer = context.Regression.Trainers.OnlineGradientDescent(labelColumnName = "Label", featureColumnName = "Features")
    *)
    
    let modelBuilder = pipeline.Append(trainer)

    let model = modelBuilder.Fit(trainData)

    let predictionEngine = context.Model.CreatePredictionEngine<HousingData,HousingPrediction>(model)

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

    (* The trained model is now something that can be used against data.
    We can pull one row from the data just to show how this is put together.
    Again, we use a record type to define the data. *)


    let predictionTest1 = predictionEngine.Predict(test1)
    printfn "Sample prediction:"
    printfn "Predicted Value: %f" predictionTest1.MedianValue
    printfn "Actual Value   : 13.20"
    printfn ""


    (* A trained model isn't much use if it can't be passed around and used 
    elsewhere. That is where the ML.NET model save and load methods come into
    play. They are both straightforward to use. The same prediction as above
    is run, but this time on a model loaded from a file. *)

    if (doSaveModel) then
        // Save model to file
        context.Model.Save(model, trainData.Schema, "model_housing.zip")




