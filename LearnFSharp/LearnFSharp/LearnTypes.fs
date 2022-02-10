module LearnTypes

open System

/// https://fsharpforfunandprofit.com/posts/overview-of-types-in-fsharp/


///// Basic Types (not a complete list)
let s1 = "hello"
let s2 = @"dir/file"
let s3 = """ This is a "method" of quoted strings """
let s4 = $"{s1} world!"
let i1 = 23
let i2 = 5
let ui1:uint32 = 1024u
let f1 = 5.
let f2 = 5.25
let d1:double = 5.
let c1 = 'x'
let hex1 = 0xFF
let oct1 = 0o77
let bin1 = 0b1011
let dec1 = 99.99M

// Booleans values are 'true' and 'false'.
let b1 = true
let b2 = false

// Operators on booleans are 'not', '&&' and '||'.
let b3 = not b1 && (b2 || false)

// Strings use double quotes.
let string1 = "Hello"
let string2  = "world"

// String concatenation is normally done with the '+' operator.
let helloWorld = string1 + " " + string2 

// Substrings use the indexer notation.  This line extracts the first 7 characters as a substring.
// Note that like many languages, Strings are zero-indexed in F#.
let substring = helloWorld[0..6]


///// Abbrev (Alias)
type ProductCode = string
type transform<'a> = 'a -> 'a


///// Record
type Product = {code:ProductCode; price:float }
type Message<'a> = {id:int; body:'a}


///// Discriminated Union
type MeasurementUnit = Cm | Inch | Mile

type Name =
  | Nickname of string
  | FirstLast of string * string

type Shape =
    | Rectangle of width:float * length:float
    | Circle of radius:float
    | Prism of width:float * float * height:float

let getShapeArea (shape:Shape) =
  match shape with
  | Rectangle (x,y) -> Some (x * y)
  | Circle (r) -> Some (Math.PI * r**2)
  | _ -> None

let getShapeWidth shape =
  match shape with
  | Rectangle(width = w) -> w
  | Circle(radius = r) -> 2. * r
  | Prism(width = w) -> w

type Shape' =
  // The value here is the radius.
  | Circ of float
  // The value here is the side length.
  | EquilateralTriangle of double
  // The value here is the side length.
  | Square of double
  // The values here are the height and width.
  | Rect of double * double

let shape1 = Circle(5.125)
let shape2 = EquilateralTriangle(4.19)
let shape3 = Square(1.5)
let shape4 = Rectangle(10., 10.)

type Tree<'a> =
  | E
  | T of Tree<'a> * 'a * Tree<'a>


///// Struct Discriminated Unions

// Because these are value types and not reference types, there are
// extra considerations compared with reference discriminated unions:
// 1. They are copied as value types and have value type semantics.
// 2. You cannot use a recursive type definition with a multicase struct Discriminated Union.
// 3. You must provide unique case names for a multicase struct Discriminated Union.

[<Struct>]
type SingleCase = Case of string

[<Struct>]
type Multicase =
    | Case1 of Case1 : string
    | Case2 of Case2 : int
    | Case3 of Case3 : double


///// Recursive Discriminated Unions
type Expression =
    | Number of int
    | Add of Expression * Expression
    | Multiply of Expression * Expression
    | Variable of string

let rec Evaluate (env:Map<string,int>) exp =
    match exp with
    | Number n -> n
    | Add (x, y) -> Evaluate env x + Evaluate env y
    | Multiply (x, y) -> Evaluate env x * Evaluate env y
    | Variable id -> env[id]

let environment = Map [ "a", 1; "b", 2; "c", 3 ]

// Create an expression tree that represents
// the expression: a + 2 * b.
let expressionTree1 = Add(Variable "a", Multiply(Number 2, Variable "b"))

// Evaluate the expression a + 2 * b, given the
// table of values for the variables.
let result = Evaluate environment expressionTree1




///// Option type
// The option type is a discriminated union.
// type Option<'a> =
//     | Some of 'a
//     | None
let myOption1 = Some(10.0)
let myOption2 = Some("string")
let myOption3 = None

let printValue opt =
  match opt with
  | Some x -> printfn "%A" x
  | None -> printfn "No value."

printValue myOption1
printValue myOption2
printValue myOption3


///// Enum
type Gender = | Male = 1 | Female = 2



// alias type
let a:string = "hi"
let b:ProductCode = "hi"
printfn "a:'%s'  b:'%s'" a b
let t1 = (a.GetType())
let t2 = (b.GetType())
printfn "Do types match?  Does %A = %A?  %b" t1 t2 (t1=t2)     //    (Type.GetType(a)) None

// record
let p = {code="X123"; price=9.99}
let q1 = {id=1; body="hello"}
let q2 = {q1 with body="world"}

// Function doubleIt has the same signature as squareIt
let squareIt = fun n -> n * n
let doubleIt = fun n -> 2 * n

// Functions squareIt and doubleIt can be stored together in a list.
let funList = [ squareIt; doubleIt ]

// Function squareIt cannot be stored in a list together with a function
// that has a different signature, such as the following body mass
// index (BMI) calculator.
let BMICalculator = fun ht wt ->
                    (float wt / float (squareIt ht)) * 703.0

// tuple (not explicitly defined with type keyword)
let tup1 = 1,2
let tup2 = (3,4)
let ti1, ti2 = tup2       // unpack tuple into individual values
printfn "%i %i" (fst tup2) (snd tup2)
// A tuple does not require its elements to be of the same type.
let mixedTuple = ( 1, "two", 3.3 )
// Similarly, function elements in tuples can have different signatures.
let funTuple = ( squareIt, BMICalculator )
// This is a list of all tuples containing all the numbers from 0 to 99 and their squares.
let sampleTableOfSquares = [ for i in 0 .. 99 -> (i, i*i) ]

// discriminated unions
let u = Inch
let name1 = Nickname("John")
let name2 = FirstLast("John", "Madden")
let myTree = T(E,"John",E)
let rect = Rectangle(length = 1.3, width = 10.0)
let circ = Circle (1.0)
let prism = Prism(5., 2.0, height = 3.0)
let area1 = getShapeArea rect
let area2 = getShapeArea circ
let area3 = getShapeArea prism
let width1 = getShapeWidth rect
let width2 = getShapeWidth circ
let width3 = getShapeWidth prism

// enum
let g = Gender.Male



    


