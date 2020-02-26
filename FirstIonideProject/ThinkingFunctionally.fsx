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

/// Function Composition

let add2 x = x + 2
let mult3 x = x * 3
let square x = x * x

[1..10] |> List.map add2 |> printfn "%A"
[1..10] |> List.map mult3 |> printfn "%A"
[1..10] |> List.map square |> printfn "%A"

let add2ThenMult3 = add2 >> mult3
let mult3ThenSquare = mult3 >> square

[1..10] |> List.map add2ThenMult3 |> printfn "%A"
[1..10] |> List.map mult3ThenSquare |> printfn "%A"

// helper functions
let logMsg msg x = printf "%s%i" msg x; x       // without linefeed
let logMsgN msg x = printfn "%s%i" msg x; x     // with linefeed

// new composed function with logging
let mult3ThenSquareLogged =
    logMsg "before="
    >> mult3
    >> logMsg " after mult3="
    >> square
    >> logMsgN " result="

mult3ThenSquareLogged 5
[1..10] |> List.map mult3ThenSquareLogged       // apply to a whole list

let listOfFunctions = [
    mult3;
    square;
    add2;
    logMsgN "result=";
    ]

// compose all functions in the list into a single function
let allFunctions = List.reduce (>>) listOfFunctions

allFunctions 5


/// Domain-Specific Languages (DSLs)

// set up the vocabulary
type DateScale = Hour | Hours | Day | Days | Week | Weeks
type DateDirection = Ago | Hence

// define a function that matches on the vocabulary
let getDate interval scale direction =
    let absHours =
        match scale with
        | Hour | Hours -> 1 * interval
        | Day | Days -> 24 * interval
        | Week | Weeks -> 24 * 7 * interval
    let signedHours =
        match direction with
        | Ago -> -1 * absHours
        | Hence -> absHours
    System.DateTime.Now.AddHours(float signedHours)

let example1 = getDate 5 Days Ago
let example2 = getDate 1 Hour Hence



// create an underlying type
type FluentShape = {
    label: string;
    color: string;
    onClick: FluentShape->FluentShape   // a function type
}

let defaultShape =
    {label=""; color=""; onClick=fun shape->shape}

let click shape =
    shape.onClick shape

let display shape =
    printfn "My label=%s and my color=%s" shape.label shape.color
    shape   // return same shape

// helper functions which we expose as the "mini-language"
let setLabel label shape =
    {shape with FluentShape.label = label}

let setColor color shape =
    {shape with FluentShape.color = color}

// add a click action to what is already there
let appendClickAction action shape =
    {shape with FluentShape.onClick = shape.onClick >> action}

// compose the base helper functions into more complex functions
let setRedBox = setColor "red" >> setLabel "box"

// create another function by composing with previous function
let setBlueBox = setRedBox >> setColor "blue"

// make a special case of appendClickAction
let changeColorOnClick color = appendClickAction (setColor color)

// we can then combine these functions together to create objects with the desired behavior
let redBox = defaultShape |> setRedBox
let blueBox = defaultShape |> setBlueBox

// create a shape that changes color when clicked
redBox
    |> display
    |> changeColorOnClick "green"
    |> click
    |> display  // new version after the click

// create a shape that changes label and color when clicked
blueBox
    |> display
    |> appendClickAction (setLabel "box2" >> setColor "green")
    |> click
    |> display  // new version after the click



let rainbow =
    ["red";"orange";"yellow";"green";"blue";"indigo";"violet"]

let showRainbow =
    let setColorAndDisplay color = setColor color >> display
    rainbow
    |> List.map setColorAndDisplay
    |> List.reduce (>>)

// test the showRainbow function
defaultShape |> showRainbow



/// Pattern Matching

// matching tuples directly
let firstPart, secondPart, _ = (1,2,3)      // underscore means ignore

// matching lists directly
let elem1::elem2::rest = [1..10]            // ignore the warning for now

// matching lists inside a match..with
let listMatcher aList =
    match aList with
    | [] -> printfn "the list is empty"
    | [firstElement] -> printfn "the list has one element %A " firstElement
    | [first; second] -> printfn "list is %A and %A" first second
    | _ -> printfn "the list has more than two elements"

listMatcher [1;2;3;4]
listMatcher [1;2]
listMatcher [1]
listMatcher []


// create some types
type Address = { Street:string; City:string; }
type Customer = { ID:int; Name:string; Address:Address }

// create a customer
let customer1 = { ID=1; Name="Bob"; Address = { Street="123 Main"; City="NY"} }

// extract name only
let { Name=name1 } = customer1;
printfn "The customer is called %s" name1

// extract name and id
let { ID=id2; Name=name2; } = customer1
printfn "The customer called %s has id %i" name2 id2

// extract name and address
let { Name=name3; Address={ Street=street3 } } = customer1;
printfn "The customer is called %s and lives on %s" name3 street3



type PersonalName = {FirstName:string; LastName:string}
let alice1 = {FirstName="Alice"; LastName="Adams"}
let alice2 = {FirstName="Alice"; LastName="Adams"}
let bob1 = {FirstName="Bob"; LastName="Bishop"}

//test
printfn "alice1=alice2 is %A" (alice1=alice2)
printfn "alice1=bob1 is %A" (alice1=bob1)



type Suit = Club | Diamond | Spade | Heart
type Rank = Two | Three | Four | Five | Six | Seven | Eight | Nine | Ten | Jack | Queen | King | Ace

let compareCard card1 card2 =
    if card1 < card2
    then printfn "%A is greater than %A" card2 card1
    else printfn "%A is greater than %A" card1 card2

let aceHearts = Heart, Ace
let twoHearts = Heart, Two
let aceSpades = Spade, Ace

compareCard aceHearts twoHearts
compareCard twoHearts aceSpades

let hand = [ Club,Ace; Heart,Three; Heart,Ace; Spade,Jack; Diamond,Two; Diamond,Ace ]

// instant sorting!
List.sort hand |> printfn "sorted hand is (low to high) %A"

// min and max work also
List.max hand |> printfn "high card is %A"
List.min hand |> printfn "low card is %A"


/// Functions as interfaces

let addingCalculator input = input + 1

let loggingCalculator innerCalculator input =
    printfn "input is %A" input
    let result = innerCalculator input
    printfn "result is %A" result
    result

let add1 input = input + 1
let times2 input = input * 2

let genericLogger anyFunc input =
    printfn "input is %A" input     // log the input
    let result = anyFunc input      // evaluate the function
    printfn "result is %A" result   // log the result
    result                          // return the result

let add1WithLogging = genericLogger add1
let times2WithLogging = genericLogger times2

add1WithLogging 3
times2WithLogging 3

[1..5] |> List.map add1WithLogging

// same generic wrapper approach can be used for other things...here is a generic wrapper
// for timing a function
let genericTimer anyFunc input =
    let stopwatch = System.Diagnostics.Stopwatch()
    stopwatch.Start()
    let result = anyFunc input  // evalue the function
    printfn "elapsed ms is %A" stopwatch.ElapsedMilliseconds
    result

let add1WithTimer = genericTimer add1WithLogging

// test
add1WithTimer 3

// TODO: write a generic caching wrapper for a slow function, so that the value is only calculated once
// TODO: write a generic "laxy" wrapper for a function, so that the inner function is only called when a result is needed


/// The strategy pattern

type Animal(noiseMakingStrategy) =
    member this.MakeNoise =
        noiseMakingStrategy() |> printfn "Making noise %s"

// now create a cat
let meowing() = "Meow"
let cat = Animal(meowing)
cat.MakeNoise

// ...and a dog
let woofOrBark() = if (System.DateTime.Now.Second % 2 = 0)
                   then "woof" else "Bark"
let dog = Animal(woofOrBark)
dog.MakeNoise
dog.MakeNoise


/// Partial Application

let add x y = x + y

let z = add 1 2
let add42 = add 42
// use the new function
add42 2
add42 3

let genericLogger before after anyFunc input =
    before input                    // callback for custom behaior
    let result = anyFunc input
    after result                    // callback for custom behavior
    result

let add1 input = input + 1

// reuse case 1
genericLogger
    (fun x -> printf "before=%i. " x)   // function to call before
    (fun x -> printfn " after=%i." x)   // function to call after
    add1                                // main function
    2                                   // parameter

// reuse case 2
genericLogger
    (fun x -> printf "started with=%i " x)  // different callback
    (fun x -> printfn " ended with=%i" x)
    add1                                // main function
    2                                   // parameter

// define a reusable function with the "callback" functions fixed
let add1WithConsoleLogging = 
    genericLogger
        (fun x -> printf "input=%i. " x)
        (fun x -> printfn " result=%i" x)
        add1
        // last parameter NOT defined here yet!

add1WithConsoleLogging 2
add1WithConsoleLogging 3
add1WithConsoleLogging 4
[1..5] |> List.map add1WithConsoleLogging


/// Active Patterns

// create an active pattern
let (|Int|_|) str =
    match System.Int32.TryParse(str) with
    | (true,int) -> Some(int)
    | _ -> None

// create an active pattern
let (|Bool|_|) str =
    match System.Boolean.TryParse(str) with
    | (true,bool) -> Some(bool)
    | _ -> None

// Once these patterns have been set up, they can be used as part of a normal "match..with" expression.

// create a function to call the patterns
let testParse str =
    match str with
    | Int i -> printfn "The value is an int '%i'" i
    | Bool b -> printfn "The value is a bool '%b'" b
    | _ -> printfn "The value '%s' is something else" str

// test
testParse "12"
testParse "true"
testParse "abc"

// create an active pattern
open System.Text.RegularExpressions
let (|FirstRegexGroup|_|) pattern input =
    let m = Regex.Match(input,pattern)
    if (m.Success) then Some m.Groups.[1].Value else None

// create a function to call the pattern
let testRegex str =
    match str with
    | FirstRegexGroup "http://(.*?)/(.*)" host ->
        printfn "The value is a url and the host is %s" host
    | FirstRegexGroup ".*?@(.*)" host ->
        printfn "The value is an email and the host is %s" host
    | _ -> printfn "The value '%s' is something else" str

// test
testRegex "http://google.com/test"
testRegex "alice@hotmail.com"

/// FizzBuzz written using active patterns

// setup the active patterns
let (|MultOf3|_|) i = if i % 3 = 0 then Some MultOf3 else None
let (|MultOf5|_|) i = if i % 5 = 0 then Some MultOf5 else None

// the main function
let fizzBuzz i =
    match i with
    | MultOf3 & MultOf5 -> printf "FizzBuzz, "
    | MultOf3 -> printf "Fizz, "
    | MultOf5 -> printf "Buzz, "
    | _ -> printf "%i, " i

// test
[1..20] |> List.iter fizzBuzz


























#endif



