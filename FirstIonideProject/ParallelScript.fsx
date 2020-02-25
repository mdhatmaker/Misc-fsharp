


(*
#r @@"..\..\Utilities\bin\Release\Utilities.dll"
#load @@"..\..\PSeq.fs"
#load "ParallelForExample.fs";;


open System
open Microsoft.Practices.ParallelGuideSamples.BasicParallelLoops.Examples;;

let opts = { LoopBodyComplexity = 10000; NumberOfSteps = 10000 }
    VerifyResult = false; SimulateInternalError = false };;

#time "on";;

let results = Chapter2.Example01 opts;;
let results = Chapter2.Example04 opts;;
*)


/// https://fsprojects.github.io/FSharp.Collections.ParallelSeq/
#r @"C:\Users\mhatm\.nuget\packages\fsharp.collections.parallelseq\1.1.2\lib\netstandard2.0\FSharp.Collections.ParallelSeq.dll"

open FSharp.Collections.ParallelSeq
open PrimeNumbers

//let isPrime x =
//    x % 10000 = 0
    
let nums = [|1..500000|]
let finalDigitOfPrimes = 
        nums 
        |> PSeq.filter isPrime
        |> PSeq.groupBy (fun i -> i % 10)
        |> PSeq.map (fun (k, vs) -> (k, Seq.length vs))
        |> PSeq.toArray  



(*
type FactorType =
    | Factors of uint64 * uint64
    | NonFactor

let getFactorType a b =
    match a % b with
    | r when r = 0UL -> Factors(b, a/b)
    | _ -> NonFactor

match getFactorType num i with
| Factors(a, b) -> getPrimes b a (a::acc)
| NonFactor -> getPrimes num (i + 1UL) acc
*)







