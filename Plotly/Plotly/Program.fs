// Learn more about F# at http://fsharp.org


open System
open XPlot.GoogleCharts

 //type Foo =
 //       static member Bar(?x : int) = 2 * x;
 //type SearchResult1 = Option<string>  // Explicit C#-style generics 
 //type SearchResult2 = string option   // built-in postfix keyword

[<EntryPoint>]
let main argv =
    let series = [ "bars"; "bars"; "bars"; "lines" ]
    let inputs = [ Bolivia; Ecuador; Madagascar; Average ]

    inputs
    |> Chart.Combo
    |> Chart.WithOptions 
         (Options(title = "Coffee Production", series = 
            [| for typ in series -> Series(typ) |]))
    |> Chart.WithLabels 
         ["Bolivia"; "Ecuador"; "Madagascar"; "Average"]
    |> Chart.WithLegend true
    |> Chart.WithSize (600, 250) |> ignore

    // Playground for optional F# parameters
    let validInt = Some 1
    let invalidInt = None

    match validInt with 
    | Some x -> printfn "the valid value is %A" x
    | None -> printfn "the value is None" 

    [1;2;3;4]  |> List.tryFind (fun x-> x = 3)  // Some 3
    [1;2;3;4]  |> List.tryFind (fun x-> x = 10) // None


    0 // return an integer exit code
