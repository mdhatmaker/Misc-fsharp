

open System


#if INTERACTIVE
let msg = "*** Interactive ***"
#else
let msg = "*** Not Interactive ***"
#endif
printfn "%s" msg


module OtherFunctions =
    let writelog (s:string) = printfn "LOG: %s" s
    // The first argument of kprintf is a function that is used to display the 
    // resulting string after formatting (so you can pass it your writelog function):
    let writelogf fmt = Printf.kprintf writelog fmt


module BasicFunctions =
    let sampleFunction1 x = x*x + 3
    printfn "Function1 returns: %d" (sampleFunction1 4573)

    let sampleFunction2 (x:int) = 2*x*x - x/5 + 3
    printfn "Function2 returns: %d" (sampleFunction2 (7+4))     // %d to print an integer

    let sampleFunction3 x =
        if x < 100.0 then
            2.0*x*x - x/5.0 + 3.0
        else
            2.0*x*x + x/5.0 - 37.0
    printfn "Function3 returns: %f" (sampleFunction3 (6.5+4.5)) // %f to print a float
    

module Immutability =
    let number = 2
    //let number = 3  // Duplicate definition of value 'number'

    let mutable otherNumber = 2
    printfn "otherNumber is %d" otherNumber
    otherNumber <- otherNumber + 1
    printfn "otherNumber changed to %d" otherNumber


module IntegersAndNumbers =
    let sampleInteger = 176     // integer
    let sampleDouble = 4.1      // floating point
    let sampleInteger2 = (sampleInteger/4 + 5 - 7) * 4 + int sampleDouble
    let sampleNumbers = [0..99] // numbers 0 to 99
    // list of tuples containing all the numbers from 0 to 99 and their squares
    let sampleTableOfSquares = [ for i in 0..99 -> (i, i*i) ]
    printfn "The table of squares is:\n%A" sampleTableOfSquares // use %A for generic printing


module Booleans =
    let boolean1 = true
    let boolean2 = false
    let boolean3 = not boolean1 && (boolean1 || false)  // 'not', '&&' and '||'
    printfn "The boolean expression evaluates to %b" boolean3


module StringManipulation =
    let string1 = "Hello"
    let string2 = "world"
    let string3 = @"C:\Program Files\"  // '@' for verbatim string literal
    let string4 = """The computer said "hello world" when I told it to!"""
    let helloWorld = string1 + " " + string2
    printfn "%s" helloWorld             // %s to print a string value
    let substring = helloWorld.[0..6]   // first 7 characters
    printfn "%s" substring


module Tuples =
    let tuple1 = (1, 2, 3)
    let swapElems (a, b) = (b, a)
    printfn "Result of swapping (1, 2) is %A" (swapElems(1, 2))
    let tuple2 = (1, "fred", 3.1415)    // tuple of an integer, a string, and a double-precision float
    printfn "tuple1: %A\ttuple2: %A" tuple1 tuple2

    // struct tuples -- These interoperate with C#7/VB15 tuples.
    let sampleStructTuple = struct (1, 2)
    //let thisWillNotCompile: (int*int) = struct (1, 2) // no implicit conversion struct tuples <--> object tuples
    let convertFromStructTuple (struct(a, b)) = (a, b)
    let convertToStructTuple (a, b) = struct(a, b)
    printfn "Struct Tuple: %A\nReference tuple made from Struct Tuple: %A" sampleStructTuple (sampleStructTuple |> convertFromStructTuple)


module PipelinesAndComposition =
    let square x = x * x
    let addOne x = x + 1
    let isOdd x = (x % 2 <> 0)
    let numbers = [1;2;3;4;5]
    let squareOddValuesAndAddOne values =
        let odds = List.filter isOdd values
        let squares = List.map square odds
        let result = List.map addOne squares
        result
    printfn "processing %A produces: %A" numbers (squareOddValuesAndAddOne numbers)
    let squareOddValuesAndAddOneNested values =
        List.map addOne (List.map square (List.filter isOdd values))
    printfn "processing %A produces: %A" numbers (squareOddValuesAndAddOneNested numbers)
    let squareOddValuesAndAddOnePipeline values =
        values
        |> List.filter isOdd
        |> List.map square
        |> List.map addOne
    printfn "processing %A produces: %A" numbers (squareOddValuesAndAddOnePipeline numbers)
    let squareOddValuesAndAddOneShorterPipeline values =
        values
        |> List.filter isOdd
        |> List.map (fun x -> x |> square |> addOne)
    printfn "processing %A produces: %A" numbers (squareOddValuesAndAddOneShorterPipeline numbers)


module Lists =
    let list1 = []      // empty list
    let list2 = [1;2;3]
    let list3 = [       // can use newlines instead of semicolons
        1
        2
        3
    ]
    let numberList = [1..1000]
    let daysList =
        [
            for month in 1..12 do
                for day in 1..System.DateTime.DaysInMonth(2020, month) do
                    yield System.DateTime(2020, month, day)
        ]
    printfn "First 5 days of 2020 are: %A" (daysList |> List.take 5)
    let blackSquares = [
        for i in 0..7 do
            for j in 0..7 do
                if (i+j) % 2 = 1 then
                    yield (i, j)   
        ]
    let squares =
        numberList
        |> List.filter (fun x -> x % 3 = 0)
        |> List.sumBy (fun x -> x * x)
    printfn "Sum of squares: %d" squares


/// https://fsharpforfunandprofit.com/posts/printf/
module FormatPrint =
    // partial application - explicit parameters
    let printStringAndInt s i = printfn "A string: %s. An int: %i" s i
    let printHelloAndInt i = printStringAndInt "hello" i
    do printHelloAndInt 42
    // partial application - point free style
    let printInt = printfn "An int: %i"
    do printInt 42
    // Of course, printf can be used for function parameters anywhere
    // a standard function can be used:
    let doSomething printerFn x y =
        let result = x + y
        printerFn "result is" result
    let callback = printfn "%s %i"
    do doSomething callback 3 4
    // also includes higher order functions for lists, etc:
    [1..5] |> List.map (sprintf "i=%i")
    // printf supports native F# types
    let t = (1,2)
    Console.WriteLine("A tuple: {0}", t)
    printfn "A tuple: %A" t
    // record printing
    type Person = {first:string; Last:string}
    let johnDoe = {first="John"; Last="Doe"}
    Console.WriteLine("A record: {0}", johnDoe)
    printfn "A record: %A" johnDoe
    // union types printing
    type Temperature = F of int | C of int
    let freezing = F 32
    Console.WriteLine("A union: {0}", freezing)
    printfn "A union: %A" freezing
    let largeNumber = 111277611
    let formatted = largeNumber.ToString("#,000")
    printfn "%s" formatted


module Arrays =
    let array1 = [||]       // empty array
    let array2 = [|"hello";"world";"and";"hello";"world";"again"|]
    let array3 = [|1..1000|]
    let array4 = [|
        for word in array2 do
            if word.Contains("l") then
                yield word
        |]
    // this is an array initialized by index and containing the even numbers from 0 to 2000:
    let evenNumbers = Array.init 1001 (fun n -> n * 2)
    let evenNumberSlice = evenNumbers.[0..500]
    // you can loop over arrays and lists using 'for' loops:
    for word in array4 do
        printfn "word: %s" word
    // you can modify the contents of an array element using the left arrow assignment operator:
    array2.[1] <- "WORLD!"
    // you can transform arrays using 'Array.map' and other functional programming operations:
    let sumOfLengths =
        array2
        |> Array.filter (fun x -> x.StartsWith "h")
        |> Array.sumBy (fun x -> x.Length)
    printfn "Sum of lengths of words that start with 'h' is: %d" sumOfLengths


// Sequences are a logical series of elements, all of the same type.
module Sequences =
    let seq1 = Seq.empty        // empty sequence
    let seq2 = seq { yield "hello"; yield "world"; yield "and"; yield "hello"; yield "world"; yield "again" }
    let numbersSeq = seq {1..1000}
    let seq3 =
        seq { for word in seq2 do
                if word.Contains("l") then
                    yield word }
    //do Seq.map (printfn "%s") seq3
    printfn "seq3 is: %A" seq3
    let evenNumbers = Seq.init 1001 (fun n -> n * 2)
    printfn "even numbers: %A" evenNumbers
    let rnd = System.Random()
    let rec randomWalk x =
        seq {   yield x
                yield! randomWalk (x + rnd.NextDouble() - 0.5) }
    let rw = randomWalk 10.0
    // show first 100 elements of the random walk:
    let first100ValuesOfRandomWalk =
        randomWalk 5.0
        |> Seq.truncate 100
        |> Seq.toList
    printfn "First 100 elements: %A" first100ValuesOfRandomWalk


module RecursiveFunctions =
    let rec factorial n =
        if n = 0 then 1 else n * factorial (n-1)
    printfn "Factorial of 6 is: %d" (factorial 6)
    // compute greatest common factor of two integers:
    let rec greatestCommonFactor a b =
        if a = 0 then b
        elif a < b then greatestCommonFactor a (b - a)
        else greatestCommonFactor (a - b) b
    printfn "GCF of 300 and 620 is %d" (greatestCommonFactor 300 620)
    // compute sum of a list of integers using recursion:
    let rec sumList xs =
        match xs with
        | [] -> 0
        | y::ys -> y + sumList ys
    // makes 'sumList' tail recursive, using helper function and 
    let rec private sumListTailRecHelper accumulator xs =
        match xs with
        | [] -> accumulator
        | y::ys -> sumListTailRecHelper (accumulator+y) ys
    // this invokes the tail recursive helper function providing '0' as a seed to the accumulator
    let sumListTailRecursive xs = sumListTailRecHelper 0 xs
    let oneThroughTen = [1;2;3;4;5;6;7;8;9;10]
    printfn "Sum 1-10 is %d" (sumListTailRecursive oneThroughTen)
    // NOTE: F# also has full support for Tail Call Optimization, which is a way to 
    // optimize recursive calls so that they are just as fast as a loop construct.


module RecordTypes =
    type ContactCard = {
        Name     : string
        Phone    : string
        Verified : bool }
    // instantiate a record type:
    let contact1 = {
        Name = "Alf"
        Phone = "(206) 555-0157"
        Verified = false }
    // can also do this on one line with ';' separators
    let contactOnSameLine = { Name = "Alf"; Phone = "(206) 555-0157"; Verified = true }
    // use "copy-and-update" on record values:
    let contact2 = {
        contact1 with
            Phone = "(206) 555-0112"
            Verified = true }
    let showContactCard (c:ContactCard) =
        c.Name + " Phone: " + c.Phone + (if not c.Verified then " (unverified)" else "")
    printfn "Alf's Contact Card: %s" (showContactCard contact1)
    /// example of a Record with a member:
    type ContactCardAlternate =
        { Name     : string
          Phone    : string
          Address  : string
          Verified : bool }
        /// Members can implement object-oriented members.
        member this.PrintedContactCard =
            this.Name + " Phone: " + this.Phone + (if not this.Verified then " (unverified) " else " ") + this.Address
    let contactAlternate = {
        Name = "Alf"
        Phone = "(206) 555-0157"
        Verified = false
        Address = "111 Alf Street" }
    printfn "Alf's alternate contact card is %s" contactAlternate.PrintedContactCard
    // Represent a Record as struct using the [<Struct>] attribute:
    [<Struct>]
    type ContactCardStruct = {
        Name        : string
        Phone       : string
        Verified    : bool }


module DiscriminatedUnions =
    /// Suit of a playing card
    type Suit =
        | Hearts
        | Clubs
        | Diamonds
        | Spades
    
    /// A Discriminated Union can also be used to represent the rank of a playing card
    type Rank =
        | Value of int
        | Ace
        | King
        | Queen
        | Jack

        static member GetAllRanks() =
            [ yield Ace
              for i in 2..10 do yield Value i
              yield Jack
              yield Queen
              yield King ]
             
    /// Record type that combines a Suit and a Rank
    type Card = { Suit: Suit; Rank: Rank }

    /// Compute a list representing all cards in the deck
    let fullDeck =
        [ for suit in [ Hearts; Diamonds; Clubs; Spades ] do
              for rank in Rank.GetAllRanks() do
                  yield { Suit=suit; Rank=rank } ]

    let showPlayingCard (c:Card) =
        let rankString =
            match c.Rank with
            | Ace -> "Ace"
            | King -> "King"
            | Queen -> "Queen"
            | Jack -> "Jack"
            | Value n -> string n
        let suitString =
            match c.Suit with
            | Clubs -> "clubs"
            | Diamonds -> "diamonds"
            | Spades -> "spades"
            | Hearts -> "hearts"
        rankString + " of " + suitString

    /// Print all the playing cards in a deck
    let printAllCards() =
        for card in fullDeck do
            printfn "%s" (showPlayingCard card)

    /// Additionally, you can represent DUs as structs:
    [<Struct>]
    type Shape =
        | Circle of radius: float
        | Square of side: float
        | Triangle of height: float * width: float
    // However, there are two key things to keep in mind when doing so:
    // 1. A struct DU cannot be recursively-defined.
    // 2. A struct DU must have unique names for each of its cases.


/// Single-case DUs are often used for domain modeling
module SingleCaseDU =
    type Address = Address of string
    type Name = Name of string
    type SSN = SSN of int

    // you can easily instantiate a single-case DU:
    let address = Address "111 Alf Way"
    let name = Name "Alf"
    let ssn = SSN 1234567890

    /// when you need a value, unwrap the underlying value with a simple function
    let unwrapAddress (Address a) = a
    let unwrapName (Name n) = n
    let unwrapSSN (SSN s) = s

    // Printing single-case DUs with unwrapping functions
    printfn "Address: %s, Name: %s, and SSN: %d" (address |> unwrapAddress) (name |> unwrapName) (ssn |> unwrapSSN)

/// DUs support recursive definitions, allowing you to represent
/// trees and inherently recursive data
module RecursiveDU = 
    type BST<'T> =
        | Empty
        | Node of value:'T * left: BST<'T> * right: BST<'T>

    /// Check if item exists in binary search tree
    /// (search recursively using Pattern Matching)
    let rec exists item bst =
        match bst with
        | Empty -> false
        | Node (x, left, right) ->
            if item = x then true
            elif item < x then (exists item left)
            else (exists item right)

    let rec insert item bst =
        match bst with
        | Empty -> Node(item, Empty, Empty)
        | Node(x, left, right) as node ->
            if item = x then node   // no need to insert, it already exists
            elif item < x then Node(x, insert item left, right)
            else Node(x, left, insert item right)

    let bst = Node(100, Empty, Empty)
    let bst2 = insert 90 bst
    let bst3 = insert 110 bst2
    let bst4 = insert 150 bst3
    printfn "Binary Search Tree: %A" bst4

(*
module PatternMatching =
    open System

    type Person = {
        First   : string
        Last    : string
    }
    // A Discriminated Union of 3 different kinds of employees
    type Employee =
        | Engineer of engineer: Person
        | Manager of manager: Person * reports: List<Employee>
        | Executive of executive: Person * reports: List<Employee> * assistant: Employee

    // Count everyone underneath the employee in management hierarchy
    let rec countReports(emp:Employee) =
        1 + match emp with
            | Engineer(person) ->
                0
            | Manager(person, reports) ->
                reports |> List.sumBy countReports
            | Executive(person, reports, assistant) ->
                (reports |> List.sumBy countReports) + countReports assistant

    // Find all managers/executives named "Dave" who do not have any reports
    let rec findDaveWithOpenPosition(emps:List<Employee>) =
        emps
        |> List.filter(function
                        | Manager({First = "Dave"}, []) -> true  // matches an empty list
                        | Executive({First = "Dave"}, [], _) -> true
                        | _ -> false)   // wildcard pattern

    let private parseHelper f = f >> function
        | (true, item) -> Some item
        | (false, _) -> None

    let parseDateTimeOffset = parseHelper DateTimeOffset.TryParse

    let result = parseDateTimeOffset "1970-01-01"
    match result with
    | Some dto -> printfn "It parsed!"
    | None -> printfn "It didn't parse!"

    // Define some more functions which parse with the helper function
    let parseInt = parseHelper Int32.TryParse
    let parseDouble = parseHelper Double.TryParse
    let parseTimeSpan = parseHelper TimeSpan.TryParse

    /// Use Active Patterns to partition input data into custom forms,
    /// decomposing them at the pattern match call site
    let (|Int|_|) = parseInt
    let (|Double|_|) = parseDouble
    let (|Date|_|) = parseDateTimeOffset
    let (|TimeSpan|_|) = parseTimeSpan

    let printParseReult = function
        | Int x -> printfn "%d" x
        | Double x -> printfn "%f" x
        | Date d -> printfn "%s" (d.ToString())
        | TimeSpan t -> printfn "%s" (t.ToString())
        | _ -> printfn "Nothing was parse-able!"
        
    printParseResult "12"
    printParseResult "12.045"
    printParseResult "12/28/2016"
    printParseResult "9:01PM"
    printParseResult "banana!"
*)

/// Option values are any kind of value tagged with either 'Some'
/// or 'None'. They are used extensively in F# code to represent the
/// cases where many other languages would use null references.
module OptionValues =
    // First, define a zip code via Single-case Discriminated Union
    type ZipCode = ZipCode of string

    // Next, define a type where the ZipCode is optional
    type Customer = { ZipCode: ZipCode option }

    // Next, define an interface type that represents an object to
    // compute the shipping zone for the customer's zip code, given
    // implementations for the 'getState' and 'getShippingZone' abstract methods
    type IShippingCalculator =
        abstract GetState: ZipCode -> string option
        abstract GetShippingZone: string -> int

    // Next, calculate a shipping zone for a customer using a calculator
    // instance. This uses combinators in the Option module to allow a
    // functional pipeline for transforming data with Optionals.
    let CustomerShippingZone (calculator:IShippingCalculator, customer:Customer) =
        customer.ZipCode
        |> Option.bind calculator.GetState
        |> Option.map calculator.GetShippingZone

/// Units of Measure
module UnitsOfMeasure =
    open Microsoft.FSharp.Data.UnitSystems.SI.UnitNames

    // define a unitized constant
    let sampleValue1 = 1600.0<meter>

    [<Measure>]
    type mile =
        // Conversion factor mile to meter
        static member asMeter = 1609.34<meter/mile>

    // define a unitized constant
    let sampleValue2 = 500.0<mile>

    // compute metric-system constant
    let sampleValue3 = sampleValue2 * mile.asMeter

    printfn "After a %.1f race I would walk %.1f miles which would be %.1f meters" sampleValue1 sampleValue2 sampleValue3

/// Classes
module DefiningClasses =
    type Vector2D(dx:double, dy:double) =
        let length = sqrt (dx*dx + dy*dy)
        member this.DX = dx
        member this.DY = dy
        member this.Length = length
        member this.Scale(k) = Vector2D(k*this.DX, k*this.DY)
    let vector1 = Vector2D(3.0, 4.0)
    let vector2 = vector1.Scale(10.0)
    printfn "Length of vector1: %f\nLength of vector2: %f" vector1.Length vector2.Length

/// Generic Classes
module DefiningGenericClasses =
    type StateTracker<'T>(initialElement:'T) =
        let mutable states = [ initialElement ]
        member this.UpdateState newState =
            states <- newState :: states
        member this.History = states
        member this.Current = states.Head
    // An 'int' instance of the state tracker class
    let tracker = StateTracker 10
    // Add a state
    tracker.UpdateState 17

/// Interfaces
module ImplementingInterfaces =
    type ReadFile() =
        let file = new System.IO.StreamReader("readme.txt")
        member this.ReadLine() = file.ReadLine()
        interface System.IDisposable with
            member this.Dispose() = file.Close()

    let interfaceImplementation =
        { new System.IDisposable with
            member this.Dispose() = printfn "disposed" }



//open FSharp.Data
//type Stocks = JsonProvider<"C:\msft.txt">


