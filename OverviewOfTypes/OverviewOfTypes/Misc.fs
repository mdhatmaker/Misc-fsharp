module TypesFieldGuide.Misc

open System



/// Value Options
/// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/value-options

let checkValue v1 v2 =
    match (v1,v2) with
    | ValueSome d1, ValueSome d2 -> printfn "Both are valid"
    | ValueSome d1, ValueNone -> printfn "First is valid"
    | ValueNone, ValueSome d2 -> printfn "Second is valid"
    | ValueNone, ValueNone -> printfn "Neither is valid"

let chk1 = checkValue (ValueSome 7) (ValueSome 11)
let chk2 = checkValue (ValueSome 7) ValueNone
let chk3 = checkValue ValueNone (ValueSome 7)
let chk4 = checkValue ValueNone ValueNone









