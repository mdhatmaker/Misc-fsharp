module Scratch


/// https://fsharpforfunandprofit.com/posts/key-concepts/
/// https://fsharpforfunandprofit.com/posts/correctness-immutability/








/// SETS AND MAPS
/// https://en.wikibooks.org/wiki/F_Sharp_Programming/Sets_and_Maps
let set1 = Set.empty.Add(1).Add(2).Add(7)
let set2 = Set.ofList ["Mercury"; "Venus"; "Earth"; "Mars"; "Jupiter"; "Saturn"; "Uranus"; "Neptune"]





/// LISTS
/// https://www.tutorialspoint.com/fsharp/fsharp_lists.htm

// Ways to create lists:
// 1. Using list literals
let li1 = [1;2;3;4;5;6;7;8;9;10]
// 2. Using cons (::) operator
let li2 = 1::2::3::4::5::6::7::8::9::10::[]
// 3. Using the List.init method of List module
let li3 = List.init 5 (fun index -> (index, index*index, index*index*index))
// 4. Using some syntactic constructs called List Comprehensions
let li4 = [1..10]
// 'yield' pushes a single value into a list, 'yield!' pushes a
// collection of values into the list:
let li5 = [ for a in 1..10 do yield (a*a) ]
let li6 = [ for a in 1..100 do if a % 3 = 0 && a % 5 = 0 then yield a ]
let li7 = [ for a in 1..3 do yield! [a..a+3] ]

// Use of Properties
let list1 = [ 2; 4; 6; 8; 10; 12; 14; 16 ]
printfn "list1.IsEmpty is %b" (list1.IsEmpty)
printfn "list1.Length is %d" (list1.Length)
printfn "list1.Head is %d" (list1.Head)
printfn "list1.Tail.Head is %d" (list1.Tail.Head)
printfn "list1.Tail.Tail.Head is %d" (list1.Tail.Tail.Head)
printfn "list1.Item(1) is %d" (list1.Item(1))


















