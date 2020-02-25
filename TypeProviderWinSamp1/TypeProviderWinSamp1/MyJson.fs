module MyJson

open FSharp.Data

(*type JsonData = JsonProvider<"""
{
    "name":"David",
    "age":42,
    "score":98.6
}
""">*)

type JsonData = JsonProvider<"""
[
{
    "name":"Nick",
    "nickname":"Slick Nick",
    "age":42,
    "score":98.6
},
{
    "name":"Shelly"
}
]
""">


let PrintJson (jd:JsonData.Root) =
    printfn "%s" jd.Name
    
    match jd.Nickname with
    | None -> "NONE"
    | Some(x) -> x
    |> printfn "%s"

    match jd.Age with
    | None -> 0
    | Some(x) -> x
    |> printfn "%i"

    match jd.Score with
    | None -> 0.0
    | Some(x) -> float x
    |> printfn "%f"

let rec PrintJsonArray a =
    match a with
    | [] -> ()
    | head::tail ->
        PrintJson head
        printfn ""
        PrintJsonArray tail
        
