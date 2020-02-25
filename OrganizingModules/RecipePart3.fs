

/// https://fsharpforfunandprofit.com/posts/recipe-part3/
/// https://fsharpforfunandprofit.com/posts/cyclic-dependencies/
/// https://fsharpforfunandprofit.com/posts/removing-cyclic-dependencies/
/// https://www.infoq.com/presentations/Value-Values/
/// https://fsharpforfunandprofit.com/books/


// Standard Design:
// 1. Presentation Layer
// 2. Domain Layer
// 3. Persistence 4. Infrastructure

// Domain Layer should NOT depend on the persistence layer ("persistence agnostic")

// In a functional design, it is very important to SEPARATE BEHAVIOR
// FROM DATA. The data types are simple and "dumb." Then separately,
// you have a number of functions that act on those data types.

// Separate each layer into two distinct parts:
// - Data Types. Data structures that are used by that layer.
// - Logic. Functions that are implemented in that layer.
// So our Functional Design will look like this:
// 1a. Presenation functions
// 1b. Presentation types
// 2a. Persistence implementation
// 2b. Persistence interfaces
// 3a. Domain functions
// 3b. Domain types
// 4. Infrastructure
// Now we move the persistence-related types to a different place
// in the hierarchy, underneath the domain functions:
// 1a. Presentation functions ---> 1b, 3b
// 1b. Presentation types ---> 3b
// 2a. Persistence implementation ---> 2b, 3b
// 3a. Domain functions ---> 2b, 3b
// 2b. Persistence interfaces
// 3b. Domain types ---> 4
// 4. Infrastructure

// Here is a sample of how a project might be organized:
// InfrastructureTypes.fs (infrastructure)
// DomainTypes.fs         (domain)
// Logger.fs              (infrastructure)
// SMTP.fs                (infrastructure)
// Validation.fs          (domain)
// SendEmail.fs           (domain)
// DbTypes.fs             (persistence)
// CustomerRepository.fs  (persistence)
// UITypes.fs             (presentation)
// UserInterface.fs       (presentation)
// UseCases.fs            (presentation)
// Main.fs



//////////////////////////////////////////////////////////////
/// SAMPLE ORGANIZATION - 
/// EACH MODULE BELOW WOULD TYPICALLY BECOME A SEPARATE FILE:
//////////////////////////////////////////////////////////////

/// ===========================================
/// Common types and functions shared across multiple projects
/// ===========================================
module CommonLibrary = 

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


/// ===========================================
/// Global types for this project
/// ===========================================
module DomainTypes = 

    open CommonLibrary 

    /// The DTO for the request
    type Request = {name:string; email:string}

    // Many more types coming soon!

/// ===========================================
/// Logging functions
/// ===========================================
module Logger = 

    open CommonLibrary 
    open DomainTypes

    let log twoTrackInput = 
        let success x = printfn "DEBUG. Success so far: %A" x; x
        let failure x = printfn "ERROR. %A" x; x
        doubleMap success failure twoTrackInput 

/// ===========================================
/// Validation functions
/// ===========================================
module Validation = 

    open CommonLibrary 
    open DomainTypes

    let validate1 input =
       if input.name = "" then Failure "Name must not be blank"
       else Success input

    let validate2 input =
       if input.name.Length > 50 then Failure "Name must not be longer than 50 chars"
       else Success input

    let validate3 input =
       if input.email = "" then Failure "Email must not be blank"
       else Success input

    // create a "plus" function for validation functions
    let (&&&) v1 v2 = 
        let addSuccess r1 r2 = r1 // return first
        let addFailure s1 s2 = s1 + "; " + s2  // concat
        plus addSuccess addFailure v1 v2 

    let combinedValidation = 
        validate1 
        &&& validate2 
        &&& validate3 

    let canonicalizeEmail input =
       { input with email = input.email.Trim().ToLower() }

/// ===========================================
/// Database functions
/// ===========================================
module CustomerRepository = 

    open CommonLibrary 
    open DomainTypes

    let updateDatabase input =
       ()   // dummy dead-end function for now

    // new function to handle exceptions
    let updateDatebaseStep = 
        tryCatch (tee updateDatabase) (fun ex -> ex.Message)

/// ===========================================
/// All the use cases or services in one place
/// ===========================================
module UseCases = 

    open CommonLibrary
    open DomainTypes

    let handleUpdateRequest = 
        Validation.combinedValidation 
        >> map Validation.canonicalizeEmail
        >> bind CustomerRepository.updateDatebaseStep
        >> Logger.log






