

open System



//let zip arr1 arr2 =
    
let loadPrices tickerSymbol =
    let a1 = [| (); (); (); (); (); |]
    let a2 = [| 100.; 101.; 102.; 101.; 104.; |]
    Seq.zip a1 a2

let ob = ()
let prices1 = [|(ob,201.);(ob,202.);(ob,203.);(ob,204.);(ob,205.)|]

    
type StockAnalyzer (lprices, days) =
    let prices =
        lprices
        |> Seq.map snd
        |> Seq.take days
    static member GetAnalyzers(tickers, days) =
        tickers
        |> Seq.map loadPrices
        |> Seq.map (fun prices -> new StockAnalyzer(prices, days))
    member s.Return =
        let lastPrice = prices |> Seq.item 0            //Seq.nth 0
        let startPrice = prices |> Seq.item (days-1)    //Seq.nth (days - 1)
        (startPrice, lastPrice, lastPrice / startPrice - 1.)




/// Given array, return the first 3 elements of the array as a TUPLE
let firstThreeToTuple (a : _[]) = (a.[0], a.[1], a.[2])

// Use "Seq.windowed n" to return a subarray of length n
seq {1 .. 6}
|> Seq.windowed 3
|> Seq.map firstThreeToTuple
|> Seq.iter (printfn "%A")

seq {1 .. 6}
|> Seq.windowed 4
|> Seq.iter (printfn "%A")



// Take a sequence and chop it up into a sequence of arrays
let chunks n (sequence: seq<_>) =
    let fold_fce (i, s) value =
        if i < n then (i+1, Seq.append s (Seq.singleton value))
                 else (  1, Seq.singleton value)

    in sequence
    |> Seq.scan (fold_fce) (0, Seq.empty)
    |> Seq.filter (fun (i,_) -> i = n)
    |> Seq.map (Seq.toArray << snd)


seq {1 .. 6}
|> chunks 3
|> Seq.map firstThreeToTuple
|> Seq.iter (printfn "%A")

(*
let taggingModel = SeqLabeler.loadModel(lthPath + 
                      "models\penn_00_18_split_dict.model");
let lemmatizer = new Lemmatizer(lthPath + "v_n_a.txt")
let input = "the rain in spain falls on the plain"

let words = Preprocessor.tokenizeSentence( input )
let tags = SeqLabeler.tagSentence( taggingModel, words )
let lemmas = Array.map2 (fun x y -> lemmatizer.lookup(x,y)) words tags
*)

let lemmatizerLookup (wt:string * string) =
     let stringify = sprintf "%s---%s" (fst wt) (snd wt)
     stringify

let words = [|"a";"b";"c";"d"|]
let tags = [|"w";"x";"y";"z"|]
let lemmas = [| for wt in Seq.zip words tags do // wt is tuple (string * string)
                  yield lemmatizerLookup wt |]  //lemmatizer.lookup wt |] 


let lemmas = Seq.zip words tags
            |> Seq.map (fun (x,y) -> lemmatizerLookup (x,y))


[1..5] |> List.map (sprintf "i=%i")


let zipAlternative = (words, tags) ||> Seq.zip
let aza = zipAlternative |> Array.ofSeq



//====================================================================================

open System.Reflection.Emit

type MyTypeA() =
    member val PropStr = "" with get, set



let mta = new MyTypeA()
mta.PropStr <- "hello, world!"
printfn "%s" mta.PropStr

//====================================================================================
    
open System
open System.Reflection
open System.Reflection.Emit
open System.Threading

let assemblyName = new AssemblyName ()
assemblyName.Name <- "HelloWorld"
let assemblyBuilder =
    Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave)
let modl = assemblyBuilder.DefineDynamicModule("HelloWorld.exe")
let typeBuilder = modl.DefineType("HelloWorldType",
                                    TypeAttributes.Public |||
                                    TypeAttributes.Class)
let methodBuilder =
    typeBuilder.DefineMethod("Main",
                             MethodAttributes.HideBySig |||
                             MethodAttributes.Static |||
                             MethodAttributes.Public,
                             typeof<Void>,
                             [|typeof<String>|])

let ilGenerator = methodBuilder.GetILGenerator()
ilGenerator.EmitWriteLine("hello world")
ilGenerator.Emit(OpCodes.Ret)
 
let helloWorldType = typeBuilder.CreateType()
helloWorldType.GetMethod("Main").Invoke(null, [|null|])
 
assemblyBuilder.SetEntryPoint(methodBuilder,
                              PEFileKinds.ConsoleApplication)
assemblyBuilder.Save("HelloWorld.exe")


//====================================================================================

// You cannot extend an existing class with new members at runtime. However, you can create
// a new class using System.Reflection.Emit that has the existing class as base class.
typeBuilder.SetParent(typeof(MyClass));
typeBuilder.DefineProperty("Prop1", PropertyAttributes.None, typeof<System.Int32>, null);







