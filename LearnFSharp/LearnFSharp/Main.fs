module Main


open CommonLibrary                      // like C# 'using' to reference a module


[<EntryPoint>]
let main argv = 

    printfn "Hello from F#"

    succeed 5                           // use a function in the CommonLibrary module

    // The "let" keyword defines an (immutable) value
    // Variable types are inferred
    let myInt = 5                       // int
    let myFloat = 3.14                  // float
    let myString = "hello"              // string

    

    // Values can be defined as "mutable"
    let mutable sum = 0
    for i = 0 to 100 do
        if i % 2 <> 0 then sum <- sum + i
    printfn "the sum of odd numbers from 0 to 100 is %A" sum


    LearnMatch.do1()
    LearnTypes.do1()
    LearnClasses.do1()
    LearnFunctions.do1()
    Collections.do1()
    LearnFiles.do1()


    0 // return an integer exit code






