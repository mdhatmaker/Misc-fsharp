module FsFunAndProfit.P002


// The "let" keyword defines an (immutable) value
let myInt = 5
let myFloat = 3.14
let myString = "hello"

// ======== Lists ========
let twoToFive = [2;3;4;5]           // Square brackets create a list with semicolon delimiters

let oneToFive = 1 :: twoToFive      // :: creates list with new 1st element
let zeroToFive = [0;1] @ twoToFive  // @ concats two lists

// IMPORTANT: commas are never used as delimiters, only semicolons!

// ======== Functions ========
// The "let" keyword also defines a named function
let square x = x * x                // Note that no parens are used
let res1 = square 3                 // Run the function (again, no parens)

let add x y = x + y                 // don't use add (x,y)! It means something completely different.
let res2 = add 2 3                  // Run the function


// to define a multiline function, just use indents
let evens list =
    let isEven x = x % 2 = 0        // Define "isEven" as an inner ("nested") funtion
    List.filter isEven list         // List.filter is a library function with two parameters: a boolean function and a list to work on


let res3 = evens oneToFive          // Now run the function


let sumOfSquaresTo100 =
    List.sum ( List.map square [1..100] )

let sumOfSquaresTo100piped =
    [1..100] |> List.map square |> List.sum     // "square" was defined earlier

let sumOfSquaresTo100withFun =
    [1..100] |> List.map (fun x -> x*x) |> List.sum


sumOfSquaresTo100 |> printfn "%d"


