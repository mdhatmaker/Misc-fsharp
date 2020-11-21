namespace Samples.FSharp.HelloWorldTypeProviderX

open System
open System.Reflection
open ProviderImplementation.ProvidedTypes
open FSharp.Core.CompilerServices
open FSharp.Quotations


/// https://fsharpforfunandprofit.com/posts/overview-of-types-in-fsharp/
/// https://docs.microsoft.com/en-us/dotnet/fsharp/tutorials/type-providers/creating-a-type-provider



// Abbrev (Alias)
type ProductCode = string
type transform<'a> = 'a -> 'a

// Tuple (not explicitly defined with type keyword)
let t1 = 1,2
let s1 = (3,4)

// Record
type Product = {code:ProductCode; price:float }
type Message<'a> = {id:int; body:'a}
//usage
let p = {code="X123"; price=9.99}
let m = {id=1; body="hello"}

// Discriminated Union
type MeasurementUnit = Cm | Inch | Mile
type Name =
    | Nickname of string
    | FirstLast of string * string
type Tree<'a> =
    | E
    | T of Tree<'a> * 'a * Tree<'a>
//usage
let u = Inch
let name = Nickname("John")
let t = T(E, "John", E)

// Enum
type Gender = | Male = 1 | Female = 2
//usage
let g = Gender.Male

// Class
type Product (code:string, price:float) =
    let isFree = price=0.0
    new (code) = Product(code,0.0)
    member this.Code = code
    member this.IsFree = isFree
//usage
let p1 = Product("X123",9.99)
let p2 = Product("X123")

// Interface
type IPrintable =
    abstract member Print : unit -> unit

// Struct
type ProductX =
    struct
        val code:string
        val price:float
        new(code) = { code = code; price = 0.0 }
    end
//usage
let pp1 = ProductX()
let pp2 = ProductX("X123")


type MyClass(initX:int) =
    let x = initX
    member this.Method() = printfn "x=%i" x




type Type1 =
    /// This is a static property
    static member StaticProperty:string = System.String.Empty

    (*/// This constructor takes no arguments
    new() = Type1("hi")                 //: unit -> Type1

    /// This constructor takes one argument
    new(data) = Type1(data)             //: data:string -> Type1*)

    /// This is an instance property
    member this.InstanceProperty : int = 0

    /// This is an instance method
    member this.InstanceMethod (x:int) = 'a'        //x:int -> char

    (*type NestedType =
        /// This is StaticProperty1 on NestedType
        static member StaticProperty1 : string

        /// This is StaticProperty100 on NestedType
        static member StaticProperty100:string*)
        
(*
type Type2 =
    ///

type Type100 =
    ///
*)


let t1 = new Type1()




// This class has a primary constructor that takes three arguments
// and an additional constructor that calls the primary constructor.
type MyClass(x0, y0, z0) =
    let mutable x = x0
    let mutable y = y0
    let mutable z = z0
    do
        printfn "Initialized object that has coordinates (%d, %d, %d)" x y z
    member this.X with get() = x and set(value) = x <- value
    member this.Y with get() = y and set(value) = y <- value
    member this.Z with get() = z and set(value) = z <- value
    new() = MyClass(0, 0, 0)

// 1. Create by using the new keyword.
let myObject1 = new MyClass(1, 2, 3)
// 2. Create without using the new keyword.
let myObject2 = MyClass(4, 5, 6)
// 3. Create by using named arguments.
let myObject3 = MyClass(x0 = 7, y0 = 8, z0 = 9)
// 4. Create by using the additional constructor.
let myObject4 = MyClass()




let a = (1,2)       // "construct"
let (a1,a2) = a     // "deconstruct"








