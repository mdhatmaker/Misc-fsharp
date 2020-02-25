module RecursionStuff


/// https://en.wikibooks.org/wiki/F_Sharp_Programming/Recursion
/// https://martin000.wordpress.com/2009/04/05/hello-world/


open System


let recursionDemo() =
    
    // FACTORIAL
    let rec factorial (n:int) =
        if n = 0 then 1
        else
            n * factorial (n-1)

    let rec factorialx = function
        | n when n < 1 -> 1
        | n -> n * factorialx (n-1)

    printfn "Factorial 6 = %i" (factorial 6)
    printfn "FactorialX 6 = %i" (factorialx 6)

    // GREATEST COMMON DIVISOR
    let rec gcd x y =
        if y = 0 then x
        else
            gcd y (x%y)

    printfn "GCD (259, 111) = %i" (gcd 259 111)

    // TAIL RECURSION
    let rec count n =
        if n = 1000000 then
            printfn "done"
        else
            if n % 1000 = 0 then
                printfn "n: %i" n

            count (n+1)     // recursive call
            ()              // NOT tail recursive because perfomrms extra work after recursive call


    (*printfn "Count (NOT tail-recursive)"
    count 0*)

    let rec fib = function
        | n when n=0I -> 0I
        | n when n=1I -> 1I
        | n -> fib(n-1I) + fib(n-2I)

    printfn "Fibonacci 10 = %A" (fib 10I)





