open System


module OtherStuff =
    /// Dictionaries with dict, .Keys and .Values
    let table = [ 'C',3;'O',15;'L',12;'I',9;'N',14 ]
    let dictionary = dict table
    let Value c =
        match dictionary.TryGetValue(c) with
        | true, v -> v
        | _ -> failwith (sprintf "letter '%c' was not in lookup table" c)
    let CalcValue name =
        name |> Seq.sumBy Value
    printfn "COLIN = %d" (CalcValue "COLIN")
    printfn "Dictionary Length: %d" dictionary.Count
    for pair in dictionary do
        printfn "%A" pair
    printfn "%A" (dictionary.Item('L'))
    let keys = dictionary.Keys
    Seq.iter (fun k -> printfn "Key = %A" k) keys
    Seq.iter (fun v -> printfn "Value = %A" v) dictionary.Values

module DoFuncs =
    let run_it() =
        let rand_list = [1;2;3]
        let rand_list2 = List.map (fun x -> x * 2) rand_list
        printfn "Double List: %A" rand_list2
      
        [5;6;7;8]
        |> List.filter (fun v -> (v % 2) = 0)
        |> List.map (fun x -> x * 2)
        |> printfn "Even Doubles: %A"

        let mult_num x = x * 3
        let add_num y = y + 5
        let mult_add = mult_num >> add_num
        let add_mult = mult_num << add_num
        printfn "mult_add = %d" (mult_add 2)
        printfn "add_mult = %d" (add_mult 2)

module DoMath =
    let run_it() =
        printfn "5 + 4 = %i" (5 + 4)
        printfn "5 * 4 = %i" (5 * 4)
        printfn "5 / 4 = %i" (5 / 4)
        printfn "5 %% 4 = %i" (5 % 4)
        printfn "5 ** 2 = %.1f" (5.0 ** 2.0)

        let number = 2
        printfn "Type : %A" (number.GetType())
        printfn "A float : %.2f" (float number)
        printfn "An int : %i" (int 3.14159)
        printfn "An int : %i" (int 3.9)     // truncating, not rounding

module StringStuff =
    let run_it() =
        let chrFromA i =
            let ascii = i + (int 'A')
            (char ascii)
        let str1 = "This is a random string"
        let str2 = @"I ignore backslashes"
        let str3 = """Now I can "quote" words in this string"""
        let str4 = str1 + " " + str2
        printfn "Length : %i" (String.length str4)
        printfn "%c" str1.[1]
        printfn "1st word : %s" (str1.[0..3])
        let commas_str = "commas"
        let upper_str = String.collect (fun c -> sprintf "%c, " c) commas_str
        printfn "Commas: %s" upper_str
        printfn "Any upper in '%s': %b" str1 (String.exists (fun c -> Char.IsUpper(c)) str1)
        printfn "Any upper in '%s': %b" commas_str (String.exists (fun c -> Char.IsUpper(c)) commas_str)
        printfn "Is Number: %b" (String.forall (fun c -> Char.IsDigit(c)) "1234")
        let string1 = String.init 10 (fun i -> i.ToString())
        let string2 = String.init 5 (fun i -> (int i).ToString())
        printfn "%s %s" string1 string2
        let string3 = String.init 5 (fun i -> (i+(int 'A')).ToString())
        let string4 = String.init 5 (fun i -> (i+(int 'A')).ToString("0 "))
        let string5 = String.init 5 (fun i -> (i+(int 'A')).ToString("000 "))
        let string6 = String.init 5 (fun i -> sprintf "%c" (chrFromA i))
        printfn "Numbers: %s    %s    %s    %s" string3 string4 string5 string6
        String.iter (fun c -> printfn "%c" c) "Print Me"
        String.collect (fun c -> sprintf "%c " c) "Print Me"
        
module LoopStuff =
    (*let magic_num = "7"
    let mutable guess = ""
    while not (magic_num.Equals(guess)) do
        printf "Guess the number: "
        guess <- Console.ReadLine()
    printfn "You guessed the number!"*)
    /// For loops
    for i = 1 to 10 do
        printf "%i " i
    printfn ""
    for i = 10 downto 1 do
        printf "%i " i
    printfn ""
    for i in  [1..10] do
        printf "%i " i
    printfn ""
    [100..110] |> List.iter (printf "%i ")
    printfn ""
    let sum = List.reduce (+) [1..10]
    printfn "Sum: %i" sum

module CondStuff =
    let age = 8
    if age < 5 then
        printfn "Preschool"
    elif age = 5 then
        printfn "Kindergarten"
    elif (age > 5) && (age <= 18) then
        let grade = age - 5
        printfn "Go to Grade %i" grade
    else
        printfn "Go to College"

    let grade2: string =
        match age with
        | age when age < 5 -> "Preschool"
        | 5 -> "Kindergarten"
        | age when (age > 5 && age <= 18) -> (age - 5).ToString()
        | _ -> "College"
    printfn "Grade2: %s" grade2

module ListStuff =
    let list1 = [1;2;3;4]
    list1 |> List.iter (printfn "Num: %i")
    printfn "%A" list1
    let list2 = 5::6::7::[]
    printfn "%A" list2
    let list3 = [1..5]
    let list4 = ['a'..'g']
    printfn "%A" list4
    let list5 = List.init 5 (fun i -> i * 2)
    printfn "%A" list5
    let list6 = [ for a in 1..5 do yield a*a ]
    let list7 = [ for a in 1..20 do if a%2=0 then yield a ]
    let list8 = [ for a in 1..3 do yield! [a .. a+2] ]
    printfn "%A\n%A\n%A" list6 list7 list8
    printfn "Length: %i" list8.Length
    printfn "Empty: %b" list8.IsEmpty
    printfn "Index 2: %c" (list4.Item(2))
    printfn "Head: %c" (list4.Head)
    printfn "Tail: %A" (list4.Tail)
    let list9 = list3 |> List.filter (fun x -> x % 2 = 0)
    let list10 = list9 |> List.map (fun x -> (x * x))
    printfn "Sorted: %A" (List.sort [5;4;3])
    printfn "Sum: %i" (List.fold (fun partialSum elem -> partialSum + elem) 0 [1;2;3])
    

type emotion =
| joy = 0
| fear = 1
| anger = 2

module EnumStuff =
    
    let show_emotion my_feeling =
        match my_feeling with
        | emotion.joy -> printfn "I'm joyful"
        | emotion.fear -> printfn "I'm fearful"
        | _ -> printfn "Some other emotion"
    show_emotion emotion.joy
    show_emotion emotion.anger


module OptionStuff =
    let divide x y =
        match y with
        | 0 -> None
        | _ -> Some (x/y)
    let showDivide x y =
        let div = (divide x y)
        if div.IsSome then
            printfn "result = %A" (div.Value)
        elif div.IsNone then
            printfn "Can't divide by zero"
        else
            printfn "Something happened" 
    showDivide 5 0
    showDivide 5 1
    showDivide 5 2    

module TupleStuff =
    let avg (w, x, y, z) : float =
        let sum = w + x + y + z
        sum / 4.0
    printfn "Avg: %f" (avg (1.0, 2.0, 3.0, 4.0))
    let my_data = ("Derek", 42, 6.25)
    let (name1, _, _) = my_data
    printfn "Name: %s" name1
    let (name2, age, _) = my_data
    printfn "Name: %s  Ago: %d" name2 age


type customer =
    { Name: string;
    Balance: float }

module RecordStuff =
    let bob = { Name="Bob Smith"; Balance=101.50 }
    printfn "%s owes us %.2f" bob.Name bob.Balance


module SeqStuff =
    // A sequence is not generated until it is required
    let seq1 = seq { 1..100 }
    let seq2 = seq { 0..2..50 }
    let seq3 = seq { 50..1 }
    printfn "%A" seq2
    Seq.toList seq2 |> List.iter (printf "%i, ")
    printfn ""
    let is_prime n =
        let rec check i =
            i > n/2 || (n % i <> 0 && check (i+1)) 
        check 2
    let prime_seq = seq { for n in 1..500 do if is_prime n then yield n }
    printfn "%A" prime_seq
    Seq.toList prime_seq |> List.iter (printfn "Prime: %i")

module MapStuff =
    let customers = 
        Map.empty.
            Add("Bob Smith", 100.50).
            Add("Sally Marks", 50.25)
    printfn "# of customers: %i" customers.Count
    let cust = customers.TryFind "Bob Smith"
    match cust with
    | Some x -> printfn "Balance: %.2f" x
    | None -> printfn "Not Found"
    printfn "Customers: %A" customers
    if customers.ContainsKey "Bob Smith" then
        printfn "Bob Smith was found"
    printfn "Bobs Balance: %.2f" customers.["Bob Smith"]
    let custs2 = Map.remove "Sally Marks" customers
    printfn "# of customers: %i" custs2.Count


let add_stuff<'T> x y =
    printfn "%A" (x + y)

module GenericStuff =
    //add_stuff<float> 5.5 2.4
    add_stuff<int> 5 2


module ExceptionStuff =
    let dividef x y =
        try
            printfn "%.2f / %.2f = %.2f" x y (x / y)
        with
            | :? System.DivideByZeroException -> printfn "Can't divide by zero!"
    dividef 5.0 4.0
    dividef 5.0 0.0
    let dividef2 x y =
        try
            if y = 0.0 then raise(DivideByZeroException "Can't divide by 0")
            else
                printfn "%.2f / %.2f = %.2f" x y (x / y)
        with
            | :? System.DivideByZeroException -> printfn "Can't divide by zero!"
    dividef2 5.0 0.0
    let dividei x y =
        try
            printfn "%i / %i = %i" x y (x / y)
        with
            | :? System.DivideByZeroException -> printfn "Can't divide by zero!"
    dividei 5 0        


type Rectangle = struct
    val Length: float
    val Width: float
    new (length, width) =
        { Length = length; Width = width }
end

module StructStuff =
    let area (shape:Rectangle) =
        shape.Length * shape.Width
    let rect = new Rectangle (5.0, 6.0)
    let rect_area = area rect
    printfn "Area: %A" rect_area


type Animal = class
    val Name: string
    val Height: float
    val Weight: float
    new (name, height, weight) =
        { Name=name; Height=height; Weight=weight }
    member x.Run =
        printfn "%s Runs" x.Name
end

type Dog(name, height, weight) =
    inherit Animal (name, height, weight)
    member x.Bark =
        printfn "%s Barks" x.Name

module ClassStuff =
    let jeffrey = new Animal ("Giraffe", 30.0, 1000.0)
    do jeffrey.Run
    let spot = new Dog ("Spot", 20.5, 40.5)
    do spot.Run
    do spot.Bark
    //do jeffrey.Bark     // ERROR: not defined for base class Animal

/////////////////////////////////////////////////////////////////////

module MoreStuff =
    let prefix prefixStr baseStr =
        prefixStr + ", " + baseStr
    prefix "Hello" "David"
    let prefixHello =
        prefix "Hello"
    prefixHello "Michael"
    let exclaim str =
        str + "!"
    let names = ["David";"Michael";"Fred";"Amy"]
    names
    |> Seq.map prefixHello
    |> Seq.map exclaim
    |> Seq.sort
    |> Seq.iter (printfn "%s")

//StringStuff.run_it()
printfn "======================================================="


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
