module SecondScript



open System.Net
open Microsoft.FSharp.Control.WebExtensions



/// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/asynchronous-workflows
module async1 =

    let urlList = [ "Microsoft.com", "http://www.microsoft.com"
                    "MSDN", "http://msdn.microsoft.com/"
                    "Bing", "http://www.bing.com"
                  ]

    let fetchAsync(name, url:string) =
        async {
            try
                let uri = new System.Uri(url)
                let webClient = new WebClient()
                let! html = webClient.AsyncDownloadString(uri)
                printfn "Read %d characters for %s" html.Length name
            with
                | ex -> printfn "%s" (ex.Message);
            }

    let runAll() =
        urlList
        |> Seq.map fetchAsync
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore

    runAll()


/// https://lorgonblog.wordpress.com/2010/03/27/f-async-on-the-client-side/


/// https://fsharpforfunandprofit.com/posts/recipe-part1/
/// https://fsharpforfunandprofit.com/posts/recipe-part2/
module programFlow1 =

    type UseCaseResult =
        | Success
        | ValidationError
        | UpdateError
        | SmtpError


    type Result<'TSuccess, 'TFailure> =
        | Success of 'TSuccess
        | Failure of 'TFailure

    type Request = {name:string; email:string}

    let validateInput input =
        if input.name = "" then Failure "Name must not be blank"
        else if input.email = "" then Failure "Email must not be blank"
        else Success input  // happy path

    (*// "adapter" function
    let bind switchFunction =
        fun twoTrackInput ->
            match twoTrackInput with
            | Success s -> switchFunction s
            | Failure f -> Failure f*)

    // another way of writing the "adapter" function
    let bind switchFunction twoTrackInput =
        match twoTrackInput with
        | Success s -> switchFunction s
        | Failure f -> Failure f
        
    (*// and a 3rd way of writing the "adapter" function
    let bind switchFunction =
        function
        | Success s -> switchFunction s
        | Failure f -> Failure f*)

    let validate1 input =
        if input.name = "" then Failure "Name must not be blank"
        else Success input

    let validate2 input =
        if input.name.Length > 50 then Failure "Name must not be longer than 50 chars"
        else Success input

    let validate3 input =
        if input.email = "" then Failure "Email must not be blank"
        else Success input

    // glue the three validation functions together
    let combinedValidation =
        // convert from switch to two-track input
        let validate2' = bind validate2
        let validate3' = bind validate3
        // connect the two-tracks together
        validate1 >> validate2' >> validate3'

    // same as above except we "inlined" the bind
    let combinedValidation2 =
        validate1
        >> bind validate2
        >> bind validate3


    // common symbol for bind function (>>=), which is used to pipe values into switch functions
    // create an infix operator
    let (>>=) twoTrackInput switchFunction =
        bind switchFunction twoTrackInput


    // use this symbol to create another implementation of the combinedValidation function:
    let combinedValidation3 x =
        x
        |> validate1        // normal pipe because validate1 has a one-track input
        >>= validate2       // validate1 results in a two-track output... so use "bind pipe"
        >>= validate3       // again, validate2 results in a two-track output... so use "bind pipe"


    // switch composition
    let (>=>) switch1 switch2 x =
        match switch1 x with
        | Success s -> switch2 s
        | Failure f -> Failure f

    (*// here's the switch composition operator rewritten:
    let (>=>) switch1 switch2 =
        switch1 >> (bind switch2)*)

    // revised combinedValidation function 
    let combinedValidation4 =
        validate1
        >=> validate2
        >=> validate3


    // test 1
    let input1 = {name=""; email=""}
    combinedValidation4 input1
    |> printfn "Result1=%A"

    // ==> Result1=Failure "Name must not be blank"

    // test 2
    let input2 = {name="Alice"; email=""}
    combinedValidation4 input2
    |> printfn "Result2=%A"

    // ==> Result2=Failure "Email must not be blank"

    // test3
    let input3 = {name="Alice"; email="good"}
    combinedValidation4 input3
    |> printfn "Result3=%A"

    // ==> Result3=Success {name="Alice"; email="good";}


    /// Bind vs switch composition
    // -Bind has one switch function parameter. It is an adapter that converts the switch
    // function into a fully two-track function (with two-track input and two-track output).
    // -Switch composition has two switch function parameters. It combines them in series to
    // make another switch function.


    // function to trim and lowercase the email address
    let canonicalizeEmail input =
        { input with email = input.email.Trim().ToLower() }

    // now we need to take the output of the one-track function and turn it into a two-track
    // result (in this case, the result will always be Success)

    // convert a normal function into a switch
    let switch f x =
        f x |> Success


    // we can easily append the canonicalizeEmail function to the end of the chain
    let usecase =
        validate1
        >=> validate2
        >=> validate3
        >=> switch canonicalizeEmail


    let goodInput = {name="Alice"; email="UPPERCASE "}
    usecase goodInput
    |> printfn "Canonicalize Good Result = %A"

    //Canonicalize Good Result = Success {name="Alice"; email="uppercase";}

    let badInput = {name=""; email="UPPERCASE "}
    usecase badInput
    |> printfn "Canonicalize Bad Result = %A"

    //Canonicalize Bad Result = Failure "Name must not be blank"


    /// Creating two-track functions from one-track functions

    // If we want to turn a one-track funciton into a two-track function directly...
    // we need an adapter block with a slot for the simple function (we call this adapter 'map').
    // IMPLEMENTATION: If the two-track input is Success, call the function, and turn its
    // output into Success. If the two-track input is Failure, bypass the function completely.

    // convert a normal function into a two-track function
    let map oneTrackFunction twoTrackInput =
        match twoTrackInput with
        | Success s -> Success (oneTrackFunction s)
        | Failure f -> Failure f

    // And here is the revised usecase function:
    let usecase2 =
        validate1
        >=> validate2
        >=> validate3
        >> map canonicalizeEmail    // normal composition
    // NOTE: Normal composition is used becauase 'map canonicalizeEmail' is a fully two-track
    // function that can be connected to the output of the validate3 switch directly.
    // In other words, for one-track functions, '>=> switch' is exactly the same as '>> map'.

    /// Converting dead-end functions to two-track functions
    /// (a "dead-end" function is a function that accepts input but has no useful output.

    // What we need to do is this:
    // -Save a copy of the input
    // -Call the function and ignore its output, if any
    // -Return the original input for passing on to the next function in the chain

    // Here's the code, which we will call 'tee' (after the UNIX tee command):
    let tee f x =
        f x |> ignore
        x

    // Once we have converted dead-end function to simple one-track pass through function, we
    // can use it in the data flow by converting it using 'switch' or 'map' as described above.
    
    // a dead-end function
    let updateDatabase input =
        ()  // dummy dead-end function for now

    let usecase3 =
        validate1
        >=> validate2
        >=> validate3
        >=> switch canonicalizeEmail
        >=> switch (tee updateDatabase)


    /// Handling exceptions

    // if our dead-end function throws an exception, we want to catch that exception and turn
    // it into a failure...we'll call this function 'tryCatch' (similar to the 'switch' function):
    let tryCatch f x =
        try
            f x |> Success
        with
        | ex -> Failure ex.Message

    let usecase4 =
        validate1
        >=> validate2
        >=> validate3
        >=> switch canonicalizeEmail
        >=> tryCatch (tee updateDatabase)


    /// Functions with two-track input

    // sometimes we need a function that handles both tracks (a logging function, for example,
    // logs errors as well as successes):
    let doubleMap successFunc failureFunc twoTrackInput =
        match twoTrackInput with
        | Success s -> Success (successFunc s)
        | Failure f -> Failure (failureFunc f)

    // we can use this function to create a simpler version of map, using id for failure function:
    let map2 successFunc =
        doubleMap successFunc id


    // let's use doubleMap to insert some logging into the data flow:
    let log twoTrackInput =
        let success x = printfn "DEBUG. Success so far: %A" x; x
        let failure x = printfn "ERROR. %A" x; x
        doubleMap success failure twoTrackInput

    let usecase5 =
        validate1
        >=> validate2
        >=> validate3
        >=> switch canonicalizeEmail
        >=> tryCatch (tee updateDatabase)
        >> log


    /// Converting a single value to a two-track value

    // simple functions that turn a single simple value into a two-track value (success or failure)

    let succeed x =
        Success x

    let fail x =
        Failure x

    // Right now these are trivial, just calling the constructor of the Result type, but when we 
    // get down to some proper coding we'll see that by using these rather than the union case
    // constructor directly, we can isolate ourselves form changes behind the scenes.


    /// Combining functions in parallel

    // we might want to run multiple switches in parallel, and combine the results

    // logic for adding two switches in parallel:
    // -take the input and apply it to each switch
    // -look at the outputs of both switches, and if both are successful, the overall result is Success
    // -if either output is a failure, then the overall result is Failure as well
    // here's the function we'll call 'plus':
    let plus' switch1 switch2 x =
        match (switch1 x),(switch2 x) with
        | Success s1,Success s2 -> Success (s1 + s2)
        | Failure f1,Success _ -> Failure f1
        | Success _,Failure f2 -> Failure f2
        | Failure f1,Failure f2 -> Failure (f1 + f2)

    // method of combining values might change, so let the caller pass in the functions that are needed:
    let plus addSuccess addFailure switch1 switch2 x =
        match (switch1 x),(switch2 x) with
        | Success s1,Success s2 -> Success (addSuccess s1 s2)
        | Failure f1,Success _ -> Failure f1
        | Success _,Failure f2 -> Failure f2
        | Failure f1,Failure f2 -> (addFailure f1 f2)

    // create a "plus" function for validation functions
    let (&&&) v1 v2 =
        let addSuccess r1 r2 = r1   // return first
        let addFailure s1 s2 = fail (s1 + "; " + s2)   // concat
        plus addSuccess addFailure v1 v2

    // using '&&&' we can create a single validation function that combines the 3 smaller validations:
    let combinedValidation5 =
        validate1
        &&& validate2
        &&& validate3

    // tidy up the dataflow function by using the 'usecase' function instead of three validation functions:
    let usecase6 = 
        combinedValidation5
        >=> switch canonicalizeEmail
        >=> tryCatch (tee updateDatabase)

    /// Like above, except OR instead of AND
    // TODO: create the OR ('|||') in the style of the existing '&&&' function

 
    /// Dynamic injection of functions

    // to add or remove functions into the flow dynamically (based on configuration settings,
    // or even the content of the data)...simplest way is two-track function to be injected
    // into the stream, and replace it with the id function if not needed
    
    type Config = {debug:bool}

    let debugLogger twoTrackInput =
        let success x = printfn "DEBUG. Success so far: %A" x; x
        let failure = id    // don't log here
        doubleMap success failure twoTrackInput

    let injectableLogger config =
        if config.debug then debugLogger else id

    let usecase7 config =
        combinedValidation
        >> map canonicalizeEmail
        >> injectableLogger config

    let inputA = {name="Alice"; email="good"}

    let releaseConfig = {debug=false}
    inputA
    |> usecase7 releaseConfig
    |> ignore

    // no output

    let debugConfig = {debug=true}
    inputA
    |> usecase7 debugConfig
    |> ignore

    // debug output
    // DEBUG. Success so far: {name = "Alice"; email = "good";}





let xs = [1;2;3;4;5]
let result =
    xs
    |> List.map (fun x -> 2 * x)
    |> List.reduce (+)







