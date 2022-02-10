module LearnDemo.Pipelines

open System


///// PIPELINE FUNCTION VALUES 1 //////////////////////////////////////////////

/// Squares a value.
let square x = x * x

/// Adds 1 to a value.
let addOne x = x + 1

/// Tests if an integer value is odd via modulo.
///
/// '<>' is a binary comparison operator that means "not equal to".
let isOdd x = x % 2 <> 0

/// A list of 5 numbers.  More on lists later.
let numbers = [ 1; 2; 3; 4; 5 ]

/// Given a list of integers, it filters out the even numbers,
/// squares the resulting odds, and adds 1 to the squared odds.
let squareOddValuesAndAddOne values = 
    let odds = List.filter isOdd values
    let squares = List.map square odds
    let result = List.map addOne squares
    result

printfn $"processing {numbers} through 'squareOddValuesAndAddOne' produces: {squareOddValuesAndAddOne numbers}"

/// A shorter way to write 'squareOddValuesAndAddOne' is to nest each
/// sub-result into the function calls themselves.
///
/// This makes the function much shorter, but it's difficult to see the
/// order in which the data is processed.
let squareOddValuesAndAddOneNested values = 
    List.map addOne (List.map square (List.filter isOdd values))

printfn $"processing {numbers} through 'squareOddValuesAndAddOneNested' produces: {squareOddValuesAndAddOneNested numbers}"

/// A preferred way to write 'squareOddValuesAndAddOne' is to use F# pipe operators.
/// This allows you to avoid creating intermediate results, but is much more readable
/// than nesting function calls like 'squareOddValuesAndAddOneNested'
let squareOddValuesAndAddOnePipeline values =
    values
    |> List.filter isOdd
    |> List.map square
    |> List.map addOne

squareOddValuesAndAddOnePipeline numbers


///// PIPELINE FUNCTION VALUES 2 //////////////////////////////////////////////

let li1 = [0..100]

li1
|> Seq.sum
