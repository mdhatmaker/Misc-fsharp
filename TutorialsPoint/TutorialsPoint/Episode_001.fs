module Episode_001

// https://youtu.be/Teak30_pXHk


let module_name = "Episode_001"


let prefix prefixStr baseStr =
    prefixStr + ", " + baseStr


let do1() =
    printfn "BEGIN MODULE %s\n" module_name

    let a = 10
    printfn "%s" (prefix "Hello" "David")
    let names = ["David"; "Maria"; "Alex"]
    printfn "%A" names

    let prefixWithHello = prefix "Hello"
    printfn "%s" (prefixWithHello "Zeke")

    names
    |> Seq.map (prefix "Hello")
    |> printfn "%A"

    let exclaim s =
        s + "!"

    names
    |> Seq.map prefixWithHello
    |> Seq.map exclaim
    |> Seq.sort
    |> printfn "%A"

    // function composition
    let bigHello = prefixWithHello >> exclaim   // vs "prefixWithHello << exclaim"

    names
    |> Seq.map bigHello
    |> Seq.sort
    |> printfn "%A"

    let hellos =
        names
        |> Seq.map (fun x -> printfn "Mapped over %s" x; bigHello x)
        |> Seq.sort
        |> Seq.iter (printfn "%s")  // without this, lazy execution ensures no output of "Mapped over" strings

    //hellos


    printfn "\nEND OF MODULE %s" module_name


    




