module RailwaySample1

open RailwayCombinatorModule



type Request = {name:string; email:string}

let validate1 input =
    if input.name = "" then Failure "Name must not be blank"
    else Success input

let validate2 input =
    if input.name.Length > 50 then Failure "Name must not be longer than 50 chars"
    else Success input

let validate3 input =
    if input.email = "" then Failure "Email must not be blank"
    else Success input

// let's use doubleMap to insert some logging into the data flow:
let log twoTrackInput =
    let success x = printfn "DEBUG. Success so far: %A" x; x
    let failure x = printfn "ERROR. %A" x; x
    doubleMap success failure twoTrackInput



let (&&&) v1 v2 =
    let addSuccess r1 r2 = r1   // return first
    let addFailure s1 s2 = s1 + "; " + s2   // concat
    plus addSuccess addFailure v1 v2

let combinedValidation =
    validate1
    &&& validate2
    &&& validate3

let canonicalizeEmail input =
    { input with email = input.email.Trim().ToLower() }

let updateDatabase input =
    ()  // dummy dead-end function for now

// new function to handle exceptions
let updateDatabaseStep =
    tryCatch (tee updateDatabase) (fun ex -> ex.Message)

let usecase =
    combinedValidation
    >> map canonicalizeEmail
    >> bind updateDatabaseStep
    >> log

