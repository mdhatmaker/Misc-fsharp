module LearnDemo


///// PIPELINE FUNCTION VALUES

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





///// PLAYING CARDS

/// The following represents the suit of a playing card.
type Suit = 
    | Hearts 
    | Clubs 
    | Diamonds 
    | Spades

/// A Discriminated Union can also be used to represent the rank of a playing card.
type Rank = 
    /// Represents the rank of cards 2 .. 10
    | Value of int
    | Ace
    | King
    | Queen
    | Jack

    /// Discriminated Unions can also implement object-oriented members.
    static member GetAllRanks() = 
        [ yield Ace
          for i in 2 .. 10 do yield Value i
          yield Jack
          yield Queen
          yield King ]
                               
/// This is a record type that combines a Suit and a Rank.
/// It's common to use both Records and Discriminated Unions when representing data.
type Card = { Suit: Suit; Rank: Rank }
          
/// This computes a list representing all the cards in the deck.
let fullDeck = 
    [ for suit in [ Hearts; Diamonds; Clubs; Spades] do
          for rank in Rank.GetAllRanks() do 
              yield { Suit=suit; Rank=rank } ]

/// This example converts a 'Card' object to a string.
let showPlayingCard (c: Card) = 
    let rankString = 
        match c.Rank with 
        | Ace -> "Ace"
        | King -> "King"
        | Queen -> "Queen"
        | Jack -> "Jack"
        | Value n -> string n
    let suitString = 
        match c.Suit with 
        | Clubs -> "clubs"
        | Diamonds -> "diamonds"
        | Spades -> "spades"
        | Hearts -> "hearts"
    rankString  + " of " + suitString

/// This example prints all the cards in a playing deck.
let printAllCards() = 
    for card in fullDeck do 
        printfn $"{showPlayingCard card}"
        




///// FIZZBUZZ

let fizzBuzz i =
    match i with
    | _ when i % 15 = 0 ->
        printf "FizzBuzz"
    | _ when i % 3 = 0 ->
        printf "Fizz"
    | _ when i % 5 = 0 ->
        printf "Buzz"
    | _ ->
        printf "%i" i

    printf "; "

// do the fizzbuzz
[1..100] |> List.iter fizzBuzz


let fizzBuzz2 rules i =
    let mutable printed = false

    for factor,label in rules do
        if i % factor = 0 then
            printed <- true
            printf "%s" label

    if not printed then
        printf "%i" i

    printf "; "



// do the fizzbuzz2
let rules = [ (3,"Fizz"); (5,"Buzz") ]
[1..100] |> List.iter (fizzBuzz2 rules)

// ...we can add additional numbers to be processed
let rules2 = [ (3,"Fizz"); (5,"Buzz"); (7,"Baz") ]
[1..105] |> List.iter (fizzBuzz2 rules2)



/// Pseudocode to demonstrate the "pipeline" version of FizzBuzz:
/// data |> handleThreeCase |> handleFiveCase |> handleOtherCases |> printResult
///
/// Also, we want the pipeline to have no side effects. The intermediate functions must
/// not print anything...instead pass any generated labels down the pipe to the end,
/// and only at that point print the results.

type Data = {i:int; label:string option}

let carbonate factor label data =
    let {i=i; label=labelSoFar} = data
    if i % factor = 0 then
        // pass on a new data record
        let newLabel =
            match labelSoFar with
            | Some s -> s + label
            | None -> label
        {data with label=Some newLabel}
    else
        // pass on the unchanged data
        data

let labelOrDefault data =
    let {i=i; label=labelSoFar} = data
    match labelSoFar with
    | Some s -> s
    | None -> sprintf "%i" i

let fizzBuzz3 i =
    {i=i; label=None}
    |> carbonate 3 "Fizz"
    |> carbonate 5 "Buzz"
    |> labelOrDefault   // convert to string
    |> printf "%s; "    // print

[1..100] |> List.iter fizzBuzz3


/// Repeat of above, but with TUPLE instead of a RECORD
/// also, replaced explicit Option matching code (match..Some..None) with some built-in
/// Option functions, 'map' and 'defaultArg'
module FizzBuzz_Pipeline_WithTuple = 

    // type Data = int * string option

    let carbonate factor label data = 
        let (i,labelSoFar) = data
        if i % factor = 0 then
            // pass on a new data record
            let newLabel = 
                labelSoFar 
                |> Option.map (fun s -> s + label)
                |> defaultArg <| label 
            (i,Some newLabel)
        else
            // pass on the unchanged data
            data

    let labelOrDefault data = 
        let (i,labelSoFar) = data
        labelSoFar 
        |> defaultArg <| sprintf "%i" i

    let fizzBuzz i = 
        (i,None)   // use tuple instead of record
        |> carbonate 3 "Fizz"
        |> carbonate 5 "Buzz"
        |> labelOrDefault     // convert to string
        |> printf "%s; "      // print

    [1..100] |> List.iter fizzBuzz

// NOTE: We use '|> defaultArg |<' because the option is the first parameter to defaultArg,
// not the second, so normal partial application won't work. ("bi-directional piping")
// OK - normal usage
// defaultArg myOption defaultValue
//
// ERROR: piping doesn't work
// myOption |> defaultArg defaultValue
//
// OK - bi-directional piping does work
// myOption |> defaultArg <| defaultValue


/// Pipline version 3
/// Q: How can we dynamically add new functions into the pipeline?
/// A: We dynamically create a function for each rule, and then combine all these functions
///    into one using composition.
let allRules =
    rules
    |> List.map (fun (factor,label) -> carbonate factor label)
    |> List.reduce (>>)

module FizzBuzz_Pipeline_WithRules = 

    let carbonate factor label data = 
        let (i,labelSoFar) = data
        if i % factor = 0 then
            // pass on a new data record
            let newLabel = 
                labelSoFar 
                |> Option.map (fun s -> s + label)
                |> defaultArg <| label 
            (i,Some newLabel)
        else
            // pass on the unchanged data
            data

    let labelOrDefault data = 
        let (i,labelSoFar) = data
        labelSoFar 
        |> defaultArg <| sprintf "%i" i

    let fizzBuzz rules i = 

        // create a single function from all the rules
        let allRules = 
            rules
            |> List.map (fun (factor,label) -> carbonate factor label)
            |> List.reduce (>>)

        (i,None)   
        |> allRules
        |> labelOrDefault     // convert to string
        |> printf "%s; "      // print

    // test
    let rules = [ (3,"Fizz"); (5,"Buzz"); (7,"Baz") ]
    [1..105] |> List.iter (fizzBuzz rules)




