module ParserCombinators

open System


let pcharA input =
    if String.IsNullOrEmpty(input) then
        (false, "")
    else if input.[0] = 'A' then
        let remaining = input.[1..]
        (true, remaining)
    else
        (false, input)

type Result<'a> =
    | Success of 'a
    | Failure of string

(*let pchar (charToMatch, input) =
    if String.IsNullOrEmpty(input) then
        Failure "No more input"
    else
        let first = input.[0]
        if first = charToMatch then
            let remaining = input.[1..]
            Success (charToMatch, remaining)
        else
            let msg = sprintf "Expecting '%c'. Got '%c'." charToMatch first
            Failure msg*)

// Version 3 - return a function

// Version 4 - wrapping the function in a type

// Type that wraps a parsing function
type Parser<'a> = Parser of (string -> Result<'a * string>)

/// Parse a single character
let pchar charToMatch =
    // define a nested inner function
    let innerFn input =
        if String.IsNullOrEmpty(input) then
            Failure "No more input"
        else
            let first = input.[0]
            if first = charToMatch then
                let remaining = input.[1..]
                Success (charToMatch, remaining)
            else
                let msg = sprintf "Expecting '%c'. Got '%c'" charToMatch first
                Failure msg
    // return the "wrapped" inner function
    Parser innerFn




(*let run parser input =
    // unwrap parser to get inner function
    let (Parser innerFn) = parser
    // call inner function with input
    innerFn input    
    
let pc1() =
    (*let digit = satisfy (fun ch -> Char.IsDigit ch) "digit"
    let point = pchar '.'
    let e = pchar 'e' <|> pchar 'E'
    let optPlusMinus = opt (pchar '-' <|> pchar '+')
    let nonZeroInt =
        digitOneNine .>>. manyChars digit
        |>> fun (first, rest) -> string first + rest
    let intPart = zero <|> nonZeroInt
    let fractionPart = point >>. manyChars1 digit
    let exponentPart = e >>. optPlusMins .>>. manyChars1 digit*)
    0*)

// =====================================
// Introducing "run"
// =====================================

// Run a parser with some input
let run parser input =
    // unwrap parser to get inner function
    let (Parser innerFn) = parser
    // call inner function with input
    innerFn input


let main() =
    //pc1()
    //printfn "%A" (pcharA "Gym")
    //printfn "%A" (pcharA "Apple")
    //printfn "%A" (pchar('G', "Gym"))
    //printfn "%A" (pchar('G', "Apple"))

    let pcharA = pchar 'A'

    let inputABC = "ABC"
    run pcharA inputABC     // Success ('A', "BC")
    let res = run pcharA inputABC
    printfn "%A" res

    let inputZBC = "ZBC"
    run pcharA inputZBC     // Failure "Expecting 'A'. Got 'Z'"

    // What is a combinator? Any function that depends only on its inputs.

    // A "combinator" library is a library designed around combining things
    // to get more complex values of the same type.

    // Parser andThen Parser => Parser
    // Parser orElse Parser => Parser
    // Parser map (transformer) => Parser

    let andThen parser1 parser2 =
        let innerFn input =
            // run parser1 with the input
            let result1 = run parser1 input

            // test the 1st parse result for Failure/Success
            match result1 with
            | Failure err ->
                Failure err    // return error from parser1
            | Success (value1, remaining1) ->
                // run parser2 with the remaining input
                let result2 = run parser2 remaining1

                // test the 2nd parse result for Failure/Success
                match result2 with
                | Failure err ->
                    Failure err
                | Success (value2, remaining2) ->
                    let combinedValue = (value1, value2)
                    Success (combinedValue, remaining2)

        // return the inner function
        Parser innerFn


    let pcharB = pchar 'B'

    //pcharA .>>. pcharB      // andThen
    //pcharA <|> pcharB       // orElse
    
    //let parseAThenBAsString = parseAThenB |>> (fun (x,y) -> string x + string y)


    0
