module LearnDemo.FizzBuzz

open FizzBuzz


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

