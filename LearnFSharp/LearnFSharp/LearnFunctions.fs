
module LearnFunctions


open System

///// The "let" keyword also defines a named function
let square x = x * x
let add x y = x + y

let result1 = square 3
let result2 = add 2 3
printfn "%d %d" result1 result2


///// Functions can use other (previously-created) functions
let add100 a = add a 100
let result4 = add100 6
result4 |> printfn "%d"


///// Partial Functions
let add100' = add 100
let result5 = add100' 25


///// In F# returns are implicit -- no "return" needed. A function always returns
///// the value of the last expression used
let getSomeValue =
    let a = 1
    let b = 2
    a + b

getSomeValue |> printfn "%d"


///// Pass a null value to a .NET method.
let ParseDateTime (str: string) =
    let (success, res) = DateTime.TryParse(str, null, System.Globalization.DateTimeStyles.AssumeUniversal)
    if success then
        Some(res)
    else
        None


///// Function composition
let function1 x = x + 1
let function2 x = x * 2
let composite = function1 >> function2

let result6 = composite 100
result6 |> printfn "%d"


///// Operator Functions
let (>>+) x y =
  (x + y) * (x * y)
let result7 = 2 >>+ 3


///// And standard operators can called as functions
let result8 = (+) 25 50
let result9 = (*) 5 10


///// Anonymous functions using "fun" keyword
let fn1 = fun x y -> x + y
let result10 = fn1 2 7


///// Recursive functions
let rec fib n =
  if n < 2 then
    1
  else
    fib (n - 1) + fib (n - 2)

let rec fib' n =
    match n with
    | 0 | 1 -> n
    | n -> fib (n-1) + fib (n-2)

let fib1 = fib 11
let fib2 = fib' 11

let rec greatestCommonFactor a b =
    if a = 0 then b
    elif a < b then greatestCommonFactor a (b - a)
    else greatestCommonFactor (a - b) b

printfn $"The Greatest Common Factor of 300 and 620 is %d{greatestCommonFactor 300 620}"

// This example computes the sum of a list of integers using recursion.
//
// '::' is used to split a list into the head and tail of the list,
// the head being the first element and the tail being the rest of the list.
let rec sumList xs =
    match xs with
    | []    -> 0
    | y::ys -> y + sumList ys

// This makes 'sumList' tail recursive, using a helper function with a result accumulator.
let rec private sumListTailRecHelper accumulator xs =
    match xs with
    | []    -> accumulator
    | y::ys -> sumListTailRecHelper (accumulator+y) ys

// This invokes the tail recursive helper function, providing '0' as a seed accumulator.
// An approach like this is common in F#.
let sumListTailRecursive xs = sumListTailRecHelper 0 xs

let oneThroughTen = [1; 2; 3; 4; 5; 6; 7; 8; 9; 10]

printfn $"The sum 1-10 is %d{sumListTailRecursive oneThroughTen}"



///// Explicit argument types
let cylinderVolume (radius : float) (length : float) : float =
  // Define a local value pi.
  let pi = 3.14159
  length * pi * radius * radius

let vol = cylinderVolume 2.0 3.0


///// Function values
let apply1 (transform : int -> int ) y = transform y

let increment x = x + 1
let result11 = apply1 increment 100

let apply2 ( f: int -> int -> int) x y = f x y

let mul x y = x * y
let result12 = apply2 mul 10 20


///// Returning a function
let makeGame target =
    // Build a lambda expression that is the function that plays the game.
    let game = fun guess ->
                   if guess = target then
                      printfn "You win!"
                   else
                      printfn "Wrong. Try again."
    // Now just return it.
    game

let playGame = makeGame 7

// Send in some guesses.
playGame 2
playGame 9
playGame 7

// The following game specifies a character instead of an integer for target.
let alphaGame = makeGame 'q'
alphaGame 'c'
alphaGame 'r'
alphaGame 'j'
alphaGame 'q'




