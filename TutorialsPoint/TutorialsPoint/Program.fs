// Learn more about F# at http://fsharp.org

open System


let hello() =
    printf "Enter your name : "
    let name = Console.ReadLine()
    printfn "Hello %s" name
    // %s %i %f %b %A %O

let hello2() =
    let big_pi = 3.141592653589793238462643383M
    printfn "PI : %f" big_pi
    printfn "PI : %M" big_pi
    printfn "%-5s %5s" "a" "b"

let bind_stuff() =
    let mutable weight = 175
    weight <- 170
    printfn "Weight : %i" weight
    let change_me = ref 10
    change_me := 50
    printfn "Change : %i" ! change_me


let do_funcs() =
    let get_sum (x : int, y : int) : int = x + y
    printfn "5 + 7 = %i" (get_sum(5, 7))

    let rec factorial x =
        if x < 1 then 1
        else x * factorial (x - 1)
    printfn "Factorial 4 : %i" (factorial 4)

    let rand_list = [1;2;3]
    let rand_list2 = List.map (fun x -> x * 2) rand_list
    printfn "Double List : %A" rand_list2

    [5;6;7;8]
    |> List.filter (fun v -> (v % 2) = 0)
    |> List.map (fun x -> x * 2)
    |> printfn "Even Doubles : %A"

    let mult_num x = x * 3
    let add_num y = y + 5
    let mult_add = mult_num >> add_num
    let add_mult = mult_num << add_num
    printfn "mult_add : %i" (mult_add 10)
    printfn "add_mult : %i" (add_mult 10)

let do_math() =
    printfn "5 ** 2 = %.1f" (5.0 ** 2.0)
    let number = 2
    printfn "Type : %A" (number.GetType())
    printfn "A Float : %.2f" (float number)
    printfn "An Int : %i" (int 3.14)

    // Also cos, sin, tan, acos, asin, atan, cosh, sinh, tanh
    printfn "abs -1 : %i" (abs -1)
    printfn "ceil 4.5 : %f" (ceil 4.5)
    printfn "floor 4.5 : %f" (floor 4.5)
    printfn "log 2.71828 : %f" (log 2.71828)
    printfn "log10 1000 : %f" (log10 1000.0)
    printfn "sqrt 25 : %f" (sqrt 25.0)

let string_stuff() =
    let str1 = "This is a random string"
    let str2 = @"I ignore backslashes"      // verbatim string
    let str3 = """ "I ignore double quotes and backslashes" """
    let str4 = str1 + " " + str2
    printfn "Length : %i" (String.length str4)
    printfn "%c" str1.[1]   // character at index 1
    printfn "1st Word : %s" (str1.[0..3])   // range of characters 0-3 (first 4 chars)
    let upper_str = String.collect (fun c -> sprintf "%c, " c) "commas"
    printfn "Commas : %s" upper_str
    printfn "Any upper : %b" (String.exists (fun c -> Char.IsUpper(c)) str1)
    printfn "Number : %b" (String.forall (fun c -> Char.IsDigit(c)) "1234")



[<EntryPoint>]
let main argv =
    //printfn "Hello World from F#!"
    //do_funcs()
    //do_math()
    //string_stuff()

    //Episode_003.do1()

    tp1.main()
    tp2.main()
    tp3.main()

    Console.ReadKey() |> ignore

    0 // return an integer exit code
