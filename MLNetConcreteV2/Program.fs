

open Concrete
open Housing


[<EntryPoint>]
let main argv =
    printfn "Testing ML.NET from F#\n"

    //Concrete.main()
    //Concrete.testLoadModel()

    Housing.main()
    //Housing.testLoadModel()

    0 // return an integer exit code

