module LearnMatch

/// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/value-options

// Value Options
let checkValue v1 v2 =
    match (v1,v2) with
    | ValueSome d1, ValueSome d2 -> printfn "Both are valid"
    | ValueSome d1, ValueNone -> printfn "First is valid"
    | ValueNone, ValueSome d2 -> printfn "Second is valid"
    | ValueNone, ValueNone -> printfn "Neither is valid"



// Pattern Matching
// Match..with.. is a supercharged case/switch statement
// (functions that do not return a value have return type 'unit')
let simplePatternMatch x =
    match x with
    | "a" -> printfn "x is a"
    | "b" -> printfn "x is b"
    | _ -> printfn "x is something else"    // underscore matches anything




let chk1 = checkValue (ValueSome 7) (ValueSome 11)
let chk2 = checkValue (ValueSome 7) ValueNone
let chk3 = checkValue ValueNone (ValueSome 7)
let chk4 = checkValue ValueNone ValueNone

simplePatternMatch "a"
simplePatternMatch "b"
simplePatternMatch "c"

// Some(..) and None are roughly analogous to Nullable wrappers
let validValue = Some(99)               // option<int>
let invalidValue = None                 // option<'a>

let IsValid x =
  match x with
  | Some _ -> true
  | None -> false

let b1 = IsValid validValue
let b2 = IsValid invalidValue



///// Pattern matching with records
type Point3D = { X: float; Y: float; Z: float }
let evaluatePoint (point: Point3D) =
    match point with
    | { X = 0.0; Y = 0.0; Z = 0.0 } -> printfn "Point is at the origin."
    | { X = xVal; Y = 0.0; Z = 0.0 } -> printfn "Point is on the x-axis. Value is %f." xVal
    | { X = 0.0; Y = yVal; Z = 0.0 } -> printfn "Point is on the y-axis. Value is %f." yVal
    | { X = 0.0; Y = 0.0; Z = zVal } -> printfn "Point is on the z-axis. Value is %f." zVal
    | { X = xVal; Y = yVal; Z = zVal } -> printfn "Point is at (%f, %f, %f)." xVal yVal zVal

evaluatePoint { X = 0.0; Y = 0.0; Z = 0.0 }
evaluatePoint { X = 100.0; Y = 0.0; Z = 0.0 }
evaluatePoint { X = 10.0; Y = 0.0; Z = -1.0 }

