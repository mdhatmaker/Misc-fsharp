module RailwayCombinatorModule


/// https://fsharpforfunandprofit.com/posts/recipe-part2/


/// Guidelines
///
/// Use double-track railway as your underlying model for dataflow situations.
/// Create a function for each step in the use case. The function for each step can in turn be built from smaller functions (e.g. the validation functions).
/// Use standard composition (>>) to connect the functions.
/// If you need to insert a switch into the flow, use bind.
/// If you need to insert a single-track function into the flow, use map.
/// If you need to insert other types of functions into the flow, create an appropriate adapter block and use it.


// the two-track type
type Result<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure

// convert a single value into a two-track result
let succeed x = 
    Success x

// convert a single value into a two-track result
let fail x = 
    Failure x

// apply either a success function or failure function
let either successFunc failureFunc twoTrackInput =
    match twoTrackInput with
    | Success s -> successFunc s
    | Failure f -> failureFunc f

// convert a switch function into a two-track function
let bind f = 
    either f fail

// pipe a two-track value into a switch function 
let (>>=) x f = 
    bind f x

// compose two switches into another switch
let (>=>) s1 s2 = 
    s1 >> bind s2

// convert a one-track function into a switch
let switch f = 
    f >> succeed

// convert a one-track function into a two-track function
let map f = 
    either (f >> succeed) fail

// convert a dead-end function into a one-track function
let tee f x = 
    f x; x 

// convert a one-track function into a switch with exception handling
let tryCatch f exnHandler x =
    try
        f x |> succeed
    with
    | ex -> exnHandler ex |> fail

// convert two one-track functions into a two-track function
let doubleMap successFunc failureFunc =
    either (successFunc >> succeed) (failureFunc >> fail)

// add two switches in parallel
let plus addSuccess addFailure switch1 switch2 x = 
    match (switch1 x),(switch2 x) with
    | Success s1,Success s2 -> Success (addSuccess s1 s2)
    | Failure f1,Success _  -> Failure f1
    | Success _ ,Failure f2 -> Failure f2
    | Failure f1,Failure f2 -> Failure (addFailure f1 f2)



/// The railway track functions: A toolkit

// We can classify our functions roughly like this:
// -"constructors" are used to create new track
// -"adapters" convert one kind of track into another
// -"combiners" link sections of track together to make a bigger piece of track

/// Here is a list of these functions we can call a "combinatory library":
///
/// Concept	  Description
/// succeed	  A constructor that takes a one-track value and creates a two-track value on the Success branch. In other contexts, this might also be called return or pure.
/// fail	  A constructor that takes a one-track value and creates a two-track value on the Failure branch.
/// bind	  An adapter that takes a switch function and creates a new function that accepts two-track values as input.
/// >>=	      An infix version of bind for piping two-track values into switch functions.
/// >>	      Normal composition. A combiner that takes two normal functions and creates a new function by connecting them in series.
/// >=>	      Switch composition. A combiner that takes two switch functions and creates a new switch function by connecting them in series.
/// switch	  An adapter that takes a normal one-track function and turns it into a switch function. (Also known as a "lift" in some contexts.)
/// map	      An adapter that takes a normal one-track function and turns it into a two-track function. (Also known as a "lift" in some contexts.)
/// tee	      An adapter that takes a dead-end function and turns it into a one-track function that can be used in a data flow. (Also known as tap.)
/// tryCatch  An adapter that takes a normal one-track function and turns it into a switch function, but also catches exceptions.
/// doubleMap An adapter that takes two one-track functions and turns them into a single two-track function. (Also known as bimap.)
/// plus	  A combiner that takes two switch functions and creates a new switch function by joining them in "parallel" and "adding" the results. (Also known as ++ and <+> in other contexts.)
/// &&&	      The "plus" combiner tweaked specifically for the validation functions, modelled on a binary AND.











