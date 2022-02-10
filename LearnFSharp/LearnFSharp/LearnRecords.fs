module LearnRecords


///// Labels are separated by semicolons when defined on the same line.
type Point = { X: float; Y: float; Z: float; }

///// You can define labels on their own line with or without a semicolon.
type Customer = 
    { First: string
      Last: string;
      SSN: uint32
      AccountNumber: uint32; }

///// A struct record.
[<Struct>]
type StructPoint = 
    { X: float
      Y: float
      Z: float }

let mypoint = { X = 1.0; Y = 1.0; Z = -1.0; }

type Point = { X: float; Y: float; Z: float; }
type Point3D = { X: float; Y: float; Z: float }
// Ambiguity: Point or Point3D?
let mypoint3D = { X = 1.0; Y = 1.0; Z = 0.0; }
// Fix this ambiguity by specifying the type name explicitly.
let myPoint1 = { Point.X = 1.0; Y = 1.0; Z = 0.0; }
let myPoint13D = { Point3D.X = 1.0; Y = 1.0; Z = 0.0; }

// Copy an existing record, and possibly change some of the field values.
let myPoint2 = { myPoint1 with Y = 100; Z = 2 }



type Product = {code:ProductCode; price:float }

type Message<'a> = {id:int; body:'a}

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



