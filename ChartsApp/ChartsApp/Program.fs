// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =
    printfn "   *** Charts and Quandl in F# ***"

    //ChartModule.tryIt()
    QuandlModule.tryIt()

    0 // return an integer exit code
