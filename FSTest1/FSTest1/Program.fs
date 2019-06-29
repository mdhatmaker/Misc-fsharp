

[<EntryPoint>]
let main argv = 
    let mutable sum = 0
    for i = 0 to 100 do
        if i % 2 <> 0 then sum <- sum + i
    printfn "the sum of odd numbers from 0 to 100 is %A" sum
    
    printfn "%A" argv
    0 // return an integer exit code

