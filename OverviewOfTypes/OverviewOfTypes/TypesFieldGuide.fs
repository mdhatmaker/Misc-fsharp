module TypesFieldGuide.Demo

open System

/// https://fsharpforfunandprofit.com/posts/overview-of-types-in-fsharp/


// Abbrev (Alias)
type ProductCode = string
type transform<'a> = 'a -> 'a

let a:string = "hi"
let b:ProductCode = "hi"
printfn "a:'%s'  b:'%s'" a b
let t1 = (a.GetType())
let t2 = (b.GetType())
printfn "Do types match?  Does %A = %A?  %b" t1 t2 (t1=t2)     //    (Type.GetType(a)) None


// Tuple (not explicitly defined with type keyword)
let tup1 = 1,2
let tup2 = (3,4)


// Record
type Product = {code:ProductCode; price:float }
type Message<'a> = {id:int; body:'a}

let p = {code="X123"; price=9.99}
let q = {id=1; body="hello"}


// Discriminated Union
type MeasurementUnit = Cm | Inch | Mile
type Name =
  | Nickname of string
  | FirstLast of string * string
type Tree<'a> =
  | E
  | T of Tree<'a> * 'a * Tree<'a>
let u = Inch
let name = Nickname("John")
let t = T(E,"John",E)


// Enum
type Gender = | Male = 1 | Female = 2
let g = Gender.Male


// Class
type Product (code:string, price:float) =
  let isFree = price=0.0
  new (code) = Product(code,0.0)
  member this.Code = code
  member this.IsFree = isFree
let p1 = Product("X123",9.99)
let p2 = Product("X123")




