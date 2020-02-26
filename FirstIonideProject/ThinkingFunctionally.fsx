module ThinkingFunctionally


/// https://fsharpforfunandprofit.com/series/thinking-functionally.html


#if DEBUG

printfn "  (skipping %s)" __SOURCE_FILE__

#else

let twoToFive = [2;3;4;5]

let oneToFive = 1::twoToFive            // create list with new 1st element
let zeroToFive = [0;1] @ twoToFive      // @ concats two lists


let square x = x * x
square 3

let add x y = x + y
add 2 3

let evens list =
    let isEven x = x%2 = 0              // Definie "isEven" as an inner function
    List.filter isEven list             // List.filter takes 2 params: a boolean function and a list to work on

evens oneToFive


// use parens to clarify precedence
let sumOfSquaresTo100 =
    List.sum (List.map square [1..100])

// pip output of one operation to the next using "|>"
let sumOfSquaresTo100piped =
    [1..100] |> List.map square |> List.sum


// ======== Pattern Matching ========
// Match..with.. is a supercharged case/switch statement.
let x = "a"
let simplePatternMatch x =
    match x with
    | "a" -> printfn "x is a"
    | "b" -> printfn "x is b"
    | _ -> printfn "x is something else"    // underscore matches anything

// Some(..) and None are roughly analagous to Nullable wrappers
let validValue = Some(99)
let invalidValue = None

let optionPatternMatch input =
    match input with
    | Some i -> printfn "input is an int=%d" i
    | None -> printfn "input is missing"

optionPatternMatch validValue
optionPatternMatch invalidValue

// ======== Complex Data Types ========

// Tuple types are pairs, triples, etc. Tuples use commas.
let twoTuple = 1,2
let threeTuple = "a",2,true

// Record types have named fields. Semicolons are separators.
type Person = {First:string; Last:string}
let person1 = {First="john"; Last="doe"}

// Union types have choices. Vertical bars are separators.
type Temp =
    | DegreesC of float
    | DegreesF of float
let temp = DegreesF 98.6

// Types can be combined recursively in complex ways.
// E.g. here is a union type that contains a list of the same type:
type Employee =
    | Worker of Person
    | Manager of Employee list
let jdoe = {First="John"; Last="Doe"}
let worker1 = Worker jdoe
let jsmith = {First="Jane"; Last="Smith"}
let worker2 = Worker jsmith
let mgr = Manager [worker1; worker2]

// all complex types have pretty printing built in
printfn "twoTuple=%A,\nPerson=%A,\nTemp=%A,\nEmployee=%A" twoTuple person1 temp worker1

// also sprintf/sprintfn functions for formatting data into a string (like String.Format)



let square x = x * x

(*let sumOfSquares n =
    [1..n] |> List.map square |> List.sum*)

let sumOfSquares n =
    [1..n]
    |> List.map square
    |> List.sum

sumOfSquares 100


/// Logic for quicksort-like algorithm:
/// 1. Take the first element of the list
/// 2. Find all elements in the rest of the list that
///    are less than the first element, and sort them.
/// 3. Find all elements in the rest of the list that
///    are >= than the first element, and sort them.
/// 4. Combine the three parts together to get the final result:
///    (sorted smaller elements + firstElement + sorted larger elements)

let rec quicksort' li =
    match li with
    | [] ->
        []
    | firstElem::otherElements ->
        let smallerElements =
            otherElements
            |> List.filter (fun x -> x < firstElem)
            |> quicksort
        let largerElements =
            otherElements
            |> List.filter (fun x -> x >= firstElem)
            |> quicksort

        List.concat [smallerElements; [firstElem]; largerElements]

let rec quicksort = function
    | [] -> []
    | first::rest ->
        let smaller,larger = List.partition ((>=) first) rest
        List.concat [quicksort smaller; [first]; quicksort larger]


let rec last = function
    | hd :: [] -> hd
    | hd :: tl -> last tl
    | _ -> failwith "Empty list."


let li1 = [6;3;2;5;7]
let li2 = ["z";"q";"r";"t";"b";"r"]
let li3 = [1;5;23;18;9;1;3]


printfn "%A" (quicksort li1)
printfn "%A" (quicksort li2)
printfn "%A" (quicksort li3)


let li3s = li3 |> quicksort

li1 |> quicksort |> printfn "%A"

let sumAList list =
    List.reduce (fun acc elem -> acc + elem) list

let sumAFoldingList list =
    List.fold (fun acc elem -> acc + elem) 0 list



open System.Net
open System
open System.IO

// fetch the contents of a web page
let fetchUrl callback url =
    let req = WebRequest.Create(Uri(url))
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new IO.StreamReader(stream)
    callback reader url


let myCallback (reader:IO.StreamReader) url =
    let html = reader.ReadToEnd()
    let html1000 = html.Substring(0, 100)
    printfn "Downloaded %s. First 100 is %s" url html1000
    //html    // return all the html

let fetchUrl2 = fetchUrl myCallback

let google = fetchUrl2 "http://www.google.com"
let bbc = fetchUrl2 "http://news.bbc.co.uk"


let sites = ["http://www.bing.com";
             "http://www.google.com";
             "http://www.amazon.com";
             "http://www.yahoo.com"]


sites |> List.map fetchUrl2






let square x = x * x

// functions as values
let squareClone = square
let result = [1..10] |> List.map squareClone

// functions taking other functions as parameters
let execFunction aFunc aParam = aFunc aParam
let result2 = execFunction square 12


// passing tuples
let add2 (x,y) = x + y
let add3 (x,y,z) = x + y + z
let li2 = [(1,2);(3,4);(5,6)]
let li3 = [(1,2,3);(4,5,6);(7,8,9)]

let sumOfOdds n =
    [1..n]
    |> List.filter (fun x -> x%2 <> 0)
    |> List.sum

let productUpTo n =
    [1..n]
    |> List.reduce (*)

let altSum n =
    let nums = [1..n]
    let odds = List.filter (fun x -> x%2 <> 0) nums |> List.reduce (+)
    let evens = List.filter (fun x -> x%2 = 0) nums |> List.reduce (+)
    evens - odds

let sumOfSquaresWithFold n =
    let initialValue = 0
    let action sumSoFar x = sumSoFar + (x*x)
    [1..n] |> List.fold action initialValue




type NameAndSize = {Name:string; Size:int}

let maxNameAndSize' list =
    let innerMax initialValue rest =
        let action maxSoFar x = if maxSoFar.Size < x.Size then x else maxSoFar
        rest |> List.fold action initialValue

    match list with
    | [] -> None            // handle empty lists
    | first::rest ->
        let max = innerMax first rest
        Some max

let maxNameAndSize list =
    match list with
    | [] -> None
    | _ ->
        let max = list |> List.maxBy (fun item -> item.Size)
        Some max

let nas1 = {Name="jay"; Size=40}
let nas2 = {Name="sue"; Size=35}
let nas3 = {Name="corey"; Size=55}
let nas4 = {Name="chris"; Size=50}
let liNas = [nas1;nas2;nas3;nas4]
maxNameAndSize liNas














#endif



