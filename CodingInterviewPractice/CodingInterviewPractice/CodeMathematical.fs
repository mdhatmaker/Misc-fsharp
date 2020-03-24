module CodeMathematical


// https://practice.geeksforgeeks.org/problems/print-the-pattern-set-1/1
let printPattern (n) =
    let ns =
        Seq.init n (fun x -> x+1)
        |> Seq.rev

    (*let pattern =
        ns
        |>*) 

    printfn "%A" ns


// https://practice.geeksforgeeks.org/problems/print-table/0
let printTable (n) =
    let ns = Seq.init 10 (fun x -> x+1)
    let res = ns |> Seq.map (fun x -> x * n)
    printfn "%A" res

// https://practice.geeksforgeeks.org/problems/series-ap/0
let seriesAP (n1, n2, k) =
    let diff = n2-n1
    let rec calcIt(n) =
        match n with
        | 1 -> n1
        | _ -> calcIt(n-1) + diff 
    let res = calcIt(k)
    printfn "%i" res

// https://practice.geeksforgeeks.org/problems/series-gp/0
let seriesGP (n1, n2, k) =
    let ratio = (float n2) / (float n1)
    let rec calcIt(n) =
        match n with
        | 1 -> (float n1)
        | _ -> calcIt(n-1) * ratio
    let res = calcIt(k)
    printfn "%.1f" res

// https://practice.geeksforgeeks.org/problems/closest-number/0
let closestNumber (n, m) =
    let n1 = Math.max(0, n-m)
    let n2 = Math.max(0, n+m)
    // (maximum) range we need to search is [n1 to n to n2]
    let rng1 = [n1..n] |> Seq.rev
    let rng2 = [n..n2] |> List.toSeq











