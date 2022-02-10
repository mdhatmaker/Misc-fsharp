open System
open System.IO



type Number =
    | Some of int
    | None

type Op =
    | Add
    | Sub

    
let evaluate (a:int) (op:Op) (b:int) =
    match op with
    | Add -> a + b
    | Sub -> a - b


(*let evaluate = function
    | Number(n) -> n
    | Binary(l, op, r) -> l*)


let HR () =
    printfn "\n-----------------------------------------------------------------------------------------\n"
    ()


evaluate 2 Add 3 |> printfn "%A"
evaluate 2 Sub 3 |> printfn "%i"

// Dictionary
HR()
let di = dict ["a", 1; "b", 2]
di.["a"] |> printfn "%i"

// List
HR()
let li = ["aa"; "bb"; "cc"]
li.[1] |> printfn "%s"

// Array
HR()
let arr = [|"aaa"; "bbb"; "ccc"|]
arr.[2] |> printfn "%s"

// Map
HR()
//let ma:Map<string, string> = Map.empty
let ma = Map.add "a" "valA" Map.empty
let mb = Map.add "b" "valB" ma
mb.["a"] |> printfn "%s"




File.Exists(@"C:\temp.txt") |> printfn "Exists: %b"
Path.GetExtension(@"C:\temp.txt") |> printfn "ext: %s"


