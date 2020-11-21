
// Highight code to execute then Alt+Enter (Option+Enter on Mac)

printfn "hello, world"

let add a b = a+b
let add42 a = add a 42
add42 6

// single line comments
(* multi-line comments
use a different symbol pair *)

// ======== "Variables" (but not really) ========
// The "let" keyword defines an (immutable) value
let myInt = 5
let myFloat = 3.14
let myString = "hello"  // note that no types were needed

// ======== Lists ========
let twoToFive = [2;3;4;5]           // Square brackets create a list with semicolon delimeters
let oneToFive = 1 :: twoToFive      // :: creates list with new 1st element
let zeroToFive = [0;1] @ twoToFive  // @ concats two lists

// ======== Functions ========
// The "let" keyword also defines a named function
let square x = x * x                // note that no parens are used
square 3                            // now run the function -- again, no parens

let add x y = x + y                 // don't use add (x,y)! it means something completely different
add 2 3                             // now run the function

// To define a multiline function, just use indents -- no semicolons needed
let evens list =
    let isEven x = x%2 = 0          // define "isEven" as an inner ("nested") function
    List.filter isEven list         // List.filter is a library function with two parameters: a
                                    // boolean function and a list to work on
evens oneToFive                     // now run the function

// You can use parens to clarify precendence. In this example, do "map" first,
// with two args, then do "sum" on the result. Without the parens, "List.map"
// would be passed as an arg to List.sum
let sumOfSquaresTo100 =
    List.sum ( List.map square [1..100] )

// You can pipe the output of one operation to the next using "/>"
// Here is the same sumOfSquares function written using pipes
let sumOfSquaresTo100piped =
    [1..100] |> List.map square |> List.sum     // "square" was defined earlier

// You can define lambdas (anonymous functions) using the "fun" keyword
let sumOfSquaresTo100withFun =
    [1..100] |> List.map (fun x -> x*x) |> List.sum

// In F# returns are implicit -- no "return" needed. A function always returns
// the value of the last expression used

// ======== Pattern Matching ========
// Match..with.. is a supercharged case/switch statement
let simplePatternMatch =
    let x = "a"
    match x with
    | "a" -> printfn "x is a"
    | "b" -> printfn "x is b"
    | _ -> printfn "x is something else"    // underscore matches anything

// Some(..) and None are roughly analogous to Nullable wrappers
let validValue = Some(99)
let invalidValue = None


