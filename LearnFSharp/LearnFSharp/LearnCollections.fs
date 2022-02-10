﻿module LearnCollections

open LearnFunctions
    

let square x = x * x


///// Lists

// To define a multiline function, just use indents
let evens list =
  let isEven x = x % 2 = 0
  List.filter isEven list
 
let sumOfSquaresTo100 =
  List.sum ( List.map square [1..100] )

// You can pipe the output of one operation to the next using "|>"
let sumOfSquaresTo100piped =
  [1..100] |> List.map square |> List.sum

// You can define lambdas (anonymous functions) using the "fun" keyword
let sumOfSquaresTo100withFun =
  [1..100] |> List.map (fun x -> x * x) |> List.sum

// Square the odd values of the input and add one, using F# pipe operators.
let squareAndAddOdd values =
  values
  |> List.filter (fun x -> x % 2 <> 0)
  |> List.map (fun x -> x * x + 1)
    
let twoToFive = [2;3;4;5]           // create (int) list
let oneToFive = 1 :: twoToFive      // prepend item with '::'
let zeroToFive = [0;1] @ twoToFive  // concatenate lists with '@'

let result3 = evens oneToFive       // use the evens function
printfn "%A" result3                // "%A" to display an object
    
sumOfSquaresTo100 |> printfn "%d"   // "%d" to display int value
printfn "%d" sumOfSquaresTo100piped
printfn "%d" sumOfSquaresTo100withFun

let numbers = [ 1; 2; 3; 4; 5 ]
let result = squareAndAddOdd numbers

let checkFor item =
    let functionToReturn = fun lst ->
                           List.exists (fun a -> a = item) lst
    functionToReturn
let fn1' = checkFor 7
let find1' = fn1' [1;2;3]
let find2' = fn1' [5;6;7]

let checkFor' lst item =
  List.exists (fun a -> a = item) lst

let find1 = checkFor' [1;2;3] 3
let find2 = checkFor' [1;2;3] 5

// This is a list of integers from 1 to 1000
let numberList = [ 1 .. 1000 ]  

// Lists can also be generated by computations. This is a list containing 
// all the days of the year.
//
// 'yield' is used for on-demand evaluation. More on this later in Sequences.
let daysList = 
    [ for month in 1 .. 12 do
          for day in 1 .. System.DateTime.DaysInMonth(2017, month) do 
              yield System.DateTime(2017, month, day) ]

// Computations can include conditionals.  This is a list containing the tuples
// which are the coordinates of the black squares on a chess board.
let blackSquares = 
    [ for i in 0 .. 7 do
          for j in 0 .. 7 do 
              if (i+j) % 2 = 1 then 
                  yield (i, j) ]
                  
// Lists can be transformed using 'List.map' and other functional programming combinators.
// This definition produces a new list by squaring the numbers in numberList, using the pipeline 
// operator to pass an argument to List.map.
let squares = 
    numberList 
    |> List.map (fun x -> x*x) 

// There are many other list combinations. The following computes the sum of the squares of the 
// numbers divisible by 3.
let sumOfSquares = 
    numberList
    |> List.filter (fun x -> x % 3 = 0)
    |> List.sumBy (fun x -> x * x)
    


///// Dictionaries

let table = [ 'C',3;'O',15;'L',12;'I',9;'N',14 ]
let di = dict table

let Value c =
  match di.TryGetValue(c) with
  | true, v -> v
  | _ -> failwith (sprintf "letter '%c' was not in lookup table" c)

let CalcValue name =
  name |> Seq.sumBy Value
    
printfn "COLIN = %d" (CalcValue "COLIN")
printfn "Dictionary Length: %d" di.Count
for pair in di do
  printfn "%A" pair
let valC = di.Item('C')
Seq.iter (fun k -> printfn "Key = %A" k) di.Keys
Seq.iter (fun v -> printfn "Value = %A" v) di.Values


///// Maps
let m1:Map<string,string> = Map.empty
let m2 = Map.add "xxx" "yyy" m1




///// Arrays

let arr = [| 1..100 |]
printfn "Array length: %d" arr.Length
arr[0] |> printfn "First element: %d"
arr[arr.Length-1] |> printfn "Last element: %d"

// This is The empty array.  Note that the syntax is similar to that of Lists, but uses `[| ... |]` instead.
let array1 = [| |]

// Arrays are specified using the same range of constructs as lists.
let array2 = [| "hello"; "world"; "and"; "hello"; "world"; "again" |]

// This is an array of numbers from 1 to 1000.
let array3 = [| 1 .. 1000 |]

// This is an array containing only the words "hello" and "world".
let array4 = 
    [| for word in array2 do
           if word.Contains("l") then 
               yield word |]

// This is an array initialized by index and containing the even numbers from 0 to 2000.
let evenNumbers = Array.init 1001 (fun n -> n * 2) 

// Sub-arrays are extracted using slicing notation.
let evenNumbersSlice = evenNumbers[0..30]

// You can loop over arrays and lists using 'for' loops.
for word in array4 do 
    printfn $"word: {word}"

// You can modify the contents of an array element by using the left arrow assignment operator.
//
// To learn more about this operator, see: https://docs.microsoft.com/dotnet/fsharp/language-reference/values/index#mutable-variables
array2[1] <- "WORLD!"

// You can transform arrays using 'Array.map' and other functional programming operations.
// The following calculates the sum of the lengths of the words that start with 'h'.
//
// Note that in this case, similar to Lists, array2 is not mutated by Array.filter.
let sumOfLengthsOfWords = 
    array2
    |> Array.filter (fun x -> x.StartsWith "h")
    |> Array.sumBy (fun x -> x.Length)



///// Sequences

// Sequences are a logical series of elements, all of the same type.
// These are a more general type than Lists and Arrays, capable of being your "view" into any logical series of elements. 

// This is the empty sequence.
let seq1 = Seq.empty

// This a sequence of values.
let seq2 = seq { yield "hello"; yield "world"; yield "and"; yield "hello"; yield "world"; yield "again" }

// This is an on-demand sequence from 1 to 1000.
let numbersSeq = seq { 1 .. 1000 }

// This is a sequence producing the words "hello" and "world"
let seq3 = 
    seq { for word in seq2 do
              if word.Contains("l") then 
                  yield word }

// This is a sequence producing the even numbers up to 2000.
let evenNumbers = Seq.init 1001 (fun n -> n * 2) 

let rnd = System.Random()

// This is an infinite sequence which is a random walk.
// This example uses yield! to return each element of a subsequence.
let rec randomWalk x =
    seq { yield x
          yield! randomWalk (x + rnd.NextDouble() - 0.5) }

// This example shows the first 100 elements of the random walk.
let first100ValuesOfRandomWalk = 
    randomWalk 5.0 
    |> Seq.truncate 100
    |> Seq.toList


