/// https://www.codesuji.com/2018/01/06/Tackling-Kaggle-FSharp-XGBoost/
/// https://www.kaggle.com/c/quora-question-pairs/data


System.IO.Directory.SetCurrentDirectory(__SOURCE_DIRECTORY__)
#r "../packages/FSharp.Data/lib/net40/FSharp.Data.dll"
#r "../packages/PicNet.XGBoost/lib/net40/XGBoost.dll"

//#r @"C:\GitHub\Misc-fsharp\WordStemSentiment\WordStemSentiment\bin\Debug\netcoreapp3.1\libxgboost.dll"

//#I @"C:\Users\mhatm\.nuget\packages\"
//#r @"picnet.xgboost\0.2.2\lib\libxgboost.dll"
#r @"C:\Users\mhatm\.nuget\packages\picnet.xgboost\0.2.2\lib\libxgboost.dll"

open System
open System.Data
open System.IO
open FSharp.Data
open XGBoost

/// https://www.kaggle.com/c/quora-question-pairs

/// First, Kaggle provides a train.csv which is used for training models.
/// This contains question pairs and the ground truth regarding their
/// duplicated-ness. Second, test.csv is questions pairs with no ground truth.


/// Data fields
/// id - the id of a training set question pair
/// qid1, qid2 - unique ids of each question (only available in train.csv)
/// question1, question2 - the full text of each question
/// is_duplicate - the target variable, set to 1 if question1 and
///   question2 have essentially the same meaning, and 0 otherwise.



// XGBoost is an optimized distributed gradient boosting library designed to be highly
// efficient, flexible and portable. It implements machine learning algorithms under
// the Gradient Boosting framework. XGBoost provides a parallel tree boosting (also
// known as GBDT, GBM) that solve many data science problems in a fast and accurate way.
// The same code runs on major distributed environment (Kubernetes, Hadoop, SGE, MPI,
// Dask) and can solve problems beyond billions of examples.

let data_folder = @"D:\Users\mhatm\OneDrive\data\MISC\quora-question-pairs\"

let getFullPathname filename =
    data_folder + filename


/// Percent of training dataset to use for training
/// Note: ValidationPct = 1. - TrainPct
[<Literal>]
let TrainPct = 0.8

/// Training filename
[<Literal>]
let TrainFilename = @"D:\Users\mhatm\OneDrive\data\MISC\quora-question-pairs\train.csv"
//let TrainFilename = getFullPathname "train.csv"

/// Kaggle test filename (used to generate submission)
[<Literal>]
let TestFilename = @"D:\Users\mhatm\OneDrive\data\MISC\quora-question-pairs\test.csv"
//let TestFilename = getFullPathname "test.csv"

/// Kaggle submission filename
[<Literal>]
let SubmissionFilename = @"D:\Users\mhatm\OneDrive\data\MISC\quora-question-pairs\submission.csv"
//let SubmissionFilename = getFullPathname "submission.csv"

/// Type of hyperparameter value
type ModelParameterType = | Int | Float32
/// Model hyperparameter
type ModelParameter = { Name: string; Type: ModelParameterType; Value: float }
/// Dataset Metadata (Used for feature calculation)
type Metadata = { AverageWordCount: float32 }
// Standardized row
type StandardRow = { QuestionId: int; Label: float32; Features: float32[] }

/// Training dataset
type TrainData = CsvProvider<TrainFilename>
/// Test/Submission dataset
type TestData = CsvProvider<TestFilename>

/// Sample dataset into train and validation datasets
let sample (input:CsvProvider<TrainFilename>) trainPct =
    let trainRows = int (float (input.Rows |> Seq.length) * trainPct)
    let trainData = input.Rows |> Seq.take trainRows |> Seq.toArray
    let validationData = input.Rows |> Seq.skip trainRows |> Seq.toArray
    (trainData, validationData)

/// Convert the test data format to train data format
/// Note: This is necessary because their train and test datasets differ slightly
let convertTestToTrainFormat (input:CsvProvider<TestFilename>.Row []) :(CsvProvider<TrainFilename>.Row []) =
    input
    |> Array.map (fun x -> new CsvProvider<TrainFilename>.Row(x.Test_id, 0, 0, x.Question1, x.Question2, false))


