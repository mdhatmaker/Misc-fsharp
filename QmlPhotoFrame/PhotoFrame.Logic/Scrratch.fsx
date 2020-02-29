

///////////////////////////////////////////////////////////////////////////////////////////////////
/// UPCASTING AND DOWNCASTING
/// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/casting-and-conversions
///////////////////////////////////////////////////////////////////////////////////////////////////
type Base1() =
    abstract member F : unit -> unit
    default u.F() =
        printfn "F base1"

type Derived1() =
    inherit Base1()
    override u.F() =
        printfn "F Derived1"


let d1:Derived1 = Derived1()

// Upcast to Base1
let base1 = d1 :> Base1

// This might throw an exception, unless you are sure that base1 is really a Derived1 object,
// as is the case here.
let derived1 = base1 :?> Derived1

// If you cannot be sure that b1 is a Derived1 object, use a type test, as follows:
let downcastBase1 (b1 : Base1) =
    match b1 with
    | :? Derived1 as derived1 -> derived1.F()
    | _ -> ()

downcastBase1 base1

let b1 = Base1()
downcastBase1 b1

downcastBase1 d1

// type annotation (':Base1') is required since 'upcast' by itself could not determine base class.
let b1:Base1 = upcast d1
// (type annotation also required for 'downcast' operator.
let d1:Derived1 = downcast b1




///////////////////////////////////////////////////////////////////////////////////////////////////
/// CLASSES Part 1
/// https://fsharpforfunandprofit.com/posts/classes/
///////////////////////////////////////////////////////////////////////////////////////////////////


// 1a. Classes always have some parameters passed in when they are created - the constructor
// 1b. There are ALWAYS parentheses after the class name (even if they're empty)
// 2. Also, classes MUST have functions attached to them as members

type CustomerName(firstName, middleInitial, lastName) =
    member this.FirstName = firstName           // equivalent: public string FirstName { get; private set; }
    member this.MiddleInitial = middleInitial   // equivalent: public string MiddleInitial { get; private set; }
    member this.LastName = lastName             // equivalent: public string LastName { get; private set; }

// The class declaration has the same parameters as the constructor, and the parameters automatically
// become immutable private fields that store the original values that were passed in.
//
// So in the example above, 'firstName', 'middleInitial', and 'lastName' automatically
// became immutable private fields.

// If you ever need to pass a tuple as a parameter to a constructor, you will have
// to annotate it explicitly.
type TupledConstructor(tuple:int * int) =
    let x,y = tuple
    do printfn "x=%i y=%i" x y

let myTC = new TupledConstructor(1,2)










///////////////////////////////////////////////////////////////////////////////////////////////////
/// CLASSES Part 2
/// https://fsharpforfunandprofit.com/posts/inheritance/
///////////////////////////////////////////////////////////////////////////////////////////////////


















///////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////

// create a new object that implements IDisposable
let makeResource name =
    { new System.IDisposable
      with member this.Dispose() = printfn "%s disposed" name }

let exampleUseBinding name =
    use myResource = makeResource name
    printfn "done"

let exampleLetBinding name =
    let myResource = makeResource name
    printfn "done"

exampleUseBinding "goodbye"

exampleLetBinding "goodbye"


open System

let sleepWorkflow = async {
    printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay

    // do! means to wait as well
    do! Async.Sleep 2000
    printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay
    }

Async.RunSynchronously sleepWorkflow



/// Workflows with other async workflows nested inside them.
/// Within the braces, the nested workflows can be blocked on by using
/// the let! or use! syntax.
let nestedWorkflow = async {
    printfn "Starting parent"

    // let! means wait and then bind to the childWorkflow value
    let! childWorkflow = Async.StartChild sleepWorkflow

    // give the child a chance and then keep working
    do! Async.Sleep 100
    printfn "Doing something useful while waiting "

    // block on the child
    let! result = childWorkflow

    // done
    printfn "Finished parent"
    }

// run the whole workflow
Async.RunSynchronously nestedWorkflow


///////////////////////////////////////////////////////////////////////////////////////////////////
/// AssemblyInfo attributes
///////////////////////////////////////////////////////////////////////////////////////////////////

// In a C# project, there is an AssemblyInfo.cs file that contains all the assembly level attributes.
// In F#, the equivalent way to do this is with a dummy module which contains a 'do' expression
// annotated with these attributes.

open System.Reflection

module AssembyInfo =
    [<assembly: AssemblyTitle("MyAssembly")>]
    [<assembly: AssemblyVersion("1.2.0.0")>]
    [<assembly: AssemblyFileVersion("1.2.3.4152")>]
    do ()   // do nothing -- just a placeholder for the attributes

    
open System.Runtime.InteropServices
open System.Text
open System.Windows.Forms.VisualStyles.VisualStyleElement.Tab

[<DllImport("shlwapi", CharSet = CharSet.Ansi, EntryPoint = "PathCanonicalize", SetLastError = true)>]
extern bool PathCanonicalize(StringBuilder lpszDst, string lpszSrc)

let TestPathCanonicalize() =
    let input = @"A:\name_1\.\name_2\..\name_3"
    let expected = @"A:\name_1\name_3"

    let builder = new StringBuilder(260)
    let success = PathCanonicalize(builder, input)
    let actual = builder.ToString()

    printfn "actual=%s success=%b" actual (expected = actual)

// test
TestPathCanonicalize()



/// https://stackoverflow.com/questions/17101329/f-sequence-comparison

// false -- '=' computes reference equality
printfn "%b" (seq {1..3} = seq {1..3})
// but this returns true?!? weird.
printfn "%b" (seq[1;2;3] = seq[1;2;3])

let a5 = seq {1..5}
let b5 = seq {1..5}
let b3 = seq {1..3}
let c5 = seq {2..6}
let eq = a5=(b5 |> Seq.map id)


let test a b = Seq.fold (&&) true (Seq.zip a b |> Seq.map (fun (aa,bb) -> aa=bb))
printfn "%b" (test a5 b5)   // true
printfn "%b" (test a5 c5)   // false
printfn "%b" (test a5 b3)   // careful -- doesn't correctly handle when sequences are different lengths!

/// Here is a function to compare two sequences (lists, etc.)
let eqs a b =
    Seq.length a = Seq.length b &&
    Seq.fold (&&) true (Seq.zip a b |> Seq.map (fun (aa,bb) -> aa=bb))

type CompareResult =
    | LT = -1
    | GT = 1
    | EQ = 0

let doCompare x y =
    if x < y then CompareResult.LT
    elif x > y then CompareResult.GT
    else CompareResult.EQ

let compareSequences s1 s2 =
    let cmpSequences =
        Seq.compareWith (fun elem1 elem2 ->
            if elem1 < elem2 then -1
            elif elem1 > elem2 then 1
            else 0)
    cmpSequences s1 s2
    
let sequencesEqual s1 s2 = (compareSequences s1 s2) = 0

printfn "%i %b" (compareSequences a5 b5) (sequencesEqual a5 b5)
printfn "%i %b" (compareSequences a5 c5) (sequencesEqual a5 c5)
printfn "%i %b" (compareSequences a5 b3) (sequencesEqual a5 b3)




/////////////////////////////////////////////////////////////////////////////////////////
/// Intrinsic Extensions
/// https://fsharpforfunandprofit.com/posts/type-extensions/
/////////////////////////////////////////////////////////////////////////////////////////

/// The key things to note are:
///
/// -The 'with' keyword indicates the start of the list of members
/// -The 'member' keyword shows that this is a member function (i.e. a method)
/// -The word 'this' is a placeholder for the object that is being dotted into (called
///  a “self-identifier”). The placeholder prefixes the function name, and then the function
///  then uses the same placeholder when it needs to refer to the current instance.
///  There is no requirement to use a particular word, just as long as it is consistent.
///  You could use 'this' or 'self' or 'me' or 'm' or any other word that commonly
///  indicates a self reference.

module Person = 
    type T = {First:string; Last:string} with
       // member defined with type declaration
        member this.FullName = 
            this.First + " " + this.Last

    // constructor
    let create first last = 
        {First=first; Last=last}

    // another member added later (within the same module)
    type T with 
        member this.SortableName = 
            this.Last + ", " + this.First        
// test
let person = Person.create "John" "Doe"
let fullname = person.FullName
let sortableName = person.SortableName


/////////////////////////////////////////////////////////////////////////////////////////
/// Optional Extensions
/// https://fsharpforfunandprofit.com/posts/type-extensions/
/////////////////////////////////////////////////////////////////////////////////////////

module PersonX = 
    type T = {First:string; Last:string} with
       // member defined with type declaration
        member this.FullName = 
            this.First + " " + this.Last

    // constructor
    let create first last = 
        {First=first; Last=last}

    // another member added later
    type T with 
        member this.SortableName = 
            this.Last + ", " + this.First  

// in a different module
module PersonExtensions = 

    type PersonX.T with 
    member this.UppercaseName = 
        this.FullName.ToUpper()

// test
open PersonExtensions       // will get an error if we don't bring the extension into scope
let person = PersonX.create "John" "Doe"
let uppercaseName = person.UppercaseName 



/////////////////////////////////////////////////////////////////////////////////////////
/// Extending System Types
/// https://fsharpforfunandprofit.com/posts/type-extensions/
/////////////////////////////////////////////////////////////////////////////////////////

type System.Int32 with                  // can't extend with a type abbreviation like 'int'
    member this.IsEven = this % 2 = 0

// test
let i = 20
if i.IsEven then printfn "'%i' is even" i



/////////////////////////////////////////////////////////////////////////////////////////
/// Static Members
/// https://fsharpforfunandprofit.com/posts/type-extensions/
/////////////////////////////////////////////////////////////////////////////////////////

// You can make the member functions static by:
//
// -adding the keyword static
// -dropping the this placeholder

module PersonB = 
    type T = {First:string; Last:string} with
        // member defined with type declaration
        member this.FullName = 
            this.First + " " + this.Last

        // static constructor
        static member Create first last = 
            {First=first; Last=last}
      
// test
let person = PersonB.T.Create "John" "Doe"
let fullname = person.FullName


/// And you can create static members for system types as well:

type System.Int32 with
    static member IsOdd x = x % 2 = 1

type System.Double with
    static member Pi = 3.141

//test
let result = System.Int32.IsOdd 20 
let pi = System.Double.Pi




/////////////////////////////////////////////////////////////////////////////////////////
/// Attaching Existing Functions
/// https://fsharpforfunandprofit.com/posts/type-extensions/
/////////////////////////////////////////////////////////////////////////////////////////

module PersonC = 
    // type with no members initially
    type T = {First:string; Last:string} 

    // constructor
    let create first last = 
        {First=first; Last=last}

    // standalone function 
    let fullName {First=first; Last=last} = 
        first + " " + last

    // attach preexisting function as a member 
    type T with 
        member this.FullName = fullName this
        
// test
let person = PersonC.create "John" "Doe"
let fullname = PersonC.fullName person  // functional style
let fullname2 = person.FullName        // OO style


/////////////////////////////////////////////////////////////////////////////////////////
/// Attaching Existing Functions with Multiple Parameters
/// https://fsharpforfunandprofit.com/posts/type-extensions/
/////////////////////////////////////////////////////////////////////////////////////////

module PersonD = 
    // type with no members initially
    type T = {First:string; Last:string} 

    // constructor
    let create first last = 
        {First=first; Last=last}

    // standalone function 
    let hasSameFirstAndLastName (person:T) otherFirst otherLast = 
        person.First = otherFirst && person.Last = otherLast

    // attach preexisting function as a member 
    type T with 
        member this.HasSameFirstAndLastName = hasSameFirstAndLastName this
        
// test
let person = PersonD.create "John" "Doe"
let result1 = PersonD.hasSameFirstAndLastName person "bob" "smith" // functional style
let result2 = person.HasSameFirstAndLastName "bob" "smith" // OO style

// In the example below, the 'hasSameFirstAndLastName' function has three parameters. Yet
// when we attach it, we only need to specify one! ("currying" and "partial application")





/////////////////////////////////////////////////////////////////////////////////////////
/// Tuple-form Methods
/// https://fsharpforfunandprofit.com/posts/type-extensions/
/////////////////////////////////////////////////////////////////////////////////////////

// When we have methods with more than one parameter,
// -We can use the standard (curried) form where the parameters are separated with spaces
// and partial application is supported. (more functional) 
// -We could pass in all the parameters at once, comma-separated in a single tuple. (more object-oriented)
//
// The tuple form is how F# interacts with the standard .NET libraries.

type ProductA = {SKU:string; Price: float} with

    // curried style
    member this.CurriedTotal qty discount = 
        (this.Price * float qty) - discount

    // tuple style
    member this.TupleTotal(qty,discount) = 
        (this.Price * float qty) - discount

// test
let product = {SKU="ABC"; Price=2.0}
let total1 = product.CurriedTotal 10 1.0 
let total2 = product.TupleTotal(10,1.0)

// The curried version can be partially applied:
let totalFor10 = product.CurriedTotal 10
let discounts = [1.0..5.0] 
let totalForDifferentDiscounts 
    = discounts |> List.map totalFor10 

// But the tuple approach can do a few things that that the curried one can’t, namely:
//
// -Named parameters
// -Optional parameters
// -Overloading

/// Named parameters with tuple-style parameters
let product = {SKU="ABC"; Price=2.0}
let total3 = product.TupleTotal(qty=10,discount=1.0)
let total4 = product.TupleTotal(discount=1.0, qty=10)

/// Optional parameters with tuple-style parameters

// For tuple-style methods, you can specify an optional parameter by prefixing the
// parameter name with a question mark.
//
// If the parameter is set, it comes through as 'Some' value
// If the parameter is not set, it comes through as 'None'
type ProductB = {SKU:string; Price: float} with

    // optional discount
    member this.TupleTotal2(qty,?discount) = 
        let extPrice = this.Price * float qty
        match discount with
        | None -> extPrice
        | Some discount -> extPrice - discount

// test
let product = {SKU="ABC"; Price=2.0}
let total1 = product.TupleTotal2(10)        // discount not specified
let total2 = product.TupleTotal2(10,1.0)    // discount specified

// There is a function defaultArg which takes the parameter as the first argument and
// a default for the second argument. If the parameter is set, the value is returned.
// And if not, the default value is returned.

// Here's the same code rewritten to use defaultArg:
type ProductC = {SKU:string; Price: float} with

    // optional discount
    member this.TupleTotal2(qty,?discount) = 
        let extPrice = this.Price * float qty
        let discount = defaultArg discount 0.0
        //return
        extPrice - discount



/////////////////////////////////////////////////////////////////////////////////////////
/// Method Overloading
/// https://fsharpforfunandprofit.com/posts/type-extensions/
/////////////////////////////////////////////////////////////////////////////////////////

// F# supports method overloading but only for methods (functions attached to types), and of
// these, only those using tuple-style parameter passing.
type ProductD = {SKU:string; Price: float} with

    // no discount
    member this.TupleTotal3(qty) = 
        printfn "using non-discount method"
        this.Price * float qty

    // with discount
    member this.TupleTotal3(qty, discount) = 
        printfn "using discount method"
        (this.Price * float qty) - discount

// test
let product = {SKU="ABC"; Price=2.0}
let total1 = product.TupleTotal3(10)        // discount not specified
let total2 = product.TupleTotal3(10,1.0)    // discount specified

/// There are some MAJOR downsides to using methods in F#:
/// -Methods don’t play well with type inference
/// -Methods don’t play well with higher order function
///
/// Summary: Don't use methods AT ALL if you can.






/////////////////////////////////////////////////////////////////////////////////////////
/// Control Flow Expressions (and how to avoid using them)
/// https://fsharpforfunandprofit.com/posts/type-extensions/
/////////////////////////////////////////////////////////////////////////////////////////

/// If-then-else

// bad
let f list = 
    if List.isEmpty list
    then printfn "is empty" 
    else printfn "first element is %s" (List.head list)

// much better
let f list = 
    match list with
    | [] -> printfn "is empty" 
    | x::_ -> printfn "first element is %s" x


// bad
let f list = 
    if List.isEmpty list
        then printfn "is empty" 
        elif (List.head list) > 0
            then printfn "first element is > 0" 
            else printfn "first element is <= 0" 

// much better
let f list = 
    match list with
    | [] -> printfn "is empty" 
    | x::_ when x > 0 -> printfn "first element is > 0" 
    | x::_ -> printfn "first element is <= 0" 


// If-then-else for one liners
// One of the places where if-then-else can be genuinely useful is to create simple
// one-liners for passing into other functions.
let posNeg x = if x > 0 then "+" elif x < 0 then "-" else "0"
[-5..5] |> List.map posNeg

// Returning functions
// Don’t forget that an if-then-else expression can return any value, including
// function values. For example:
let greetings =
    if (System.DateTime.Now.Hour < 12)
    then (fun name -> "good morning, " + name)
    else (fun name -> "good day, " + name)

// test
greetings "Alice"



/// Loops

// Printing something 10 times:
// bad
for i = 1 to 10 do
   printf "%i" i

// much better
[1..10] |> List.iter (printf "%i")


// Summing a list:
// bad
let sum list = 
    let mutable total = 0    // uh-oh -- mutable value 
    for e in list do
        total <- total + e   // update the mutable value
    total  

// much better
let sum list = List.reduce (+) list

// test
sum [1..10]


// Generating and printing a sequence of random numbers:
// bad
let printRandomNumbersUntilMatched matchValue maxValue =
  let mutable continueLooping = true  // another mutable value
  let randomNumberGenerator = new System.Random()
  while continueLooping do
    // Generate a random number between 1 and maxValue.
    let rand = randomNumberGenerator.Next(maxValue)
    printf "%d " rand
    if rand = matchValue then 
       printfn "\nFound a %d!" matchValue
       continueLooping <- false

// much better
let printRandomNumbersUntilMatched matchValue maxValue =
  let randomNumberGenerator = new System.Random()
  let sequenceGenerator _ = randomNumberGenerator.Next(maxValue)
  let isNotMatch = (<>) matchValue

  //create and process the sequence of rands
  Seq.initInfinite sequenceGenerator 
    |> Seq.takeWhile isNotMatch
    |> Seq.iter (printf "%d ")

  // done
  printfn "\nFound a %d!" matchValue

//test
printRandomNumbersUntilMatched 10 20


// Loops for one liners
// One of the places where loops are used in practice is as list and sequence generators:
let myList = [for x in 0..100 do if x*x < 100 then yield x ]











































 