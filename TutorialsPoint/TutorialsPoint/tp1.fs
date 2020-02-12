module tp1

// https://www.tutorialspoint.com/fsharp/fsharp_quick_guide.htm

open System

(*
// INTEGRAL DATA TYPES
F# TYPE     SIZE        EXAMPLE
sbyte       1 byte      42y -11y
byte        1 byte      42uy 200uy
int16       2 bytes     42s -11s
uint16      2 bytes     42us 200us
int/int32   4 bytes     42 -11
uint32      4 bytes     42u 200u
int64       8 bytes     42L -11L
uint64      8 bytes     42UL 200UL
bigint      >= 4 bytes  42I 1499999999999999999999999999999999I
*)
let math_int() =
    // single-byte integer
    let x = 21y
    let y = 32y
    let z = x + y
    printfn "x: %i" x
    printfn "y: %i" y
    printfn "z: %i" z

    // unsigned 8-bit natural number
    let p = 2uy
    let q = 4uy
    let r = p + q
    printfn "%i + %i = %i" p q r

    // signed 32-bit integer
    let d = 212l
    let e = 504l
    let f = d + e
    printfn "%i + %i = %i" d e f

(*
// FLOATING POINT DATA TYPES
F# TYPE     SIZE        EXAMPLE
float32     4 bytes     42.0F -11.0F
float       8 bytes     42.0 -11.0
decimal     16 bytes    42.0M -11.0M
BigRational >= 4 bytes  42N -11N
*)
let math_float() =
    // 32-bit signed floating point number
    // (7 significant digits)
    let d = 212.098f
    let e = 504.768f
    let f = d + e
    printfn "%f + %f = %f" d e f

    // 64-bit signed floating point number
    // (15-16 significant digits)
    let x = 21290.098
    let y = 50446.768
    let z = x + y
    printfn "%g + %g = %g" x y z

(*
// TEXT DATA TYPES
F# TYPE     SIZE                                EXAMPLE
char        2 bytes                             'x' '\t'
string      20 + (2 * string's length) bytes    "Hello" "World"
*)
let text_types() =
    let choice = 'y'
    let name = "Zara Ali"
    let org = "Tutorials Point"
    printfn "Choice: %c" choice
    printfn "Name: %s" name
    printfn "Organization: %s" org

(*
// OTHER DATA TYPES
F# TYPE     SIZE        EXAMPLE
bool        1 byte      true false
*)
let other_types() =
    let trueVal = true
    let falseVal = false
    printfn "True Value: %b" (trueVal)
    printfn "False Value: %b" (falseVal)



let sign num =
    if num > 0 then "positive"
    elif num < 0 then "negative"
    else "zero"



let main() =
    Console.WriteLine("\nF# DATA TYPES\n")

    Console.WriteLine("sign 5: {0}", (sign 5))

    math_int()
    math_float()
    text_types()
    other_types()





