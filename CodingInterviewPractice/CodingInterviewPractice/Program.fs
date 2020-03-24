module Main

/// https://www.geeksforgeeks.org/practice-for-cracking-any-coding-interview/


open System

[<EntryPoint>]
let main argv =
    printfn "GeeksForGeeks - Coding Interview Practice"

    //CodeMathematical.PrintPattern(3)
    //CodeMathematical.PrintTable(9)
    //CodeMathematical.SeriesAP (2, 3, 4)
    //CodeMathematical.SeriesAP (1, 2, 10)
    CodeMathematical.SeriesGP (2, 3, 1)
    CodeMathematical.SeriesGP (1, 2, 6)


    0 // return an integer exit code
