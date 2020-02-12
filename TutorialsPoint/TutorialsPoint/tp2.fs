module tp2

// https://www.tutorialspoint.com/fsharp/fsharp_quick_guide.htm

open System


let var1() =
    // use 'let' keyword for variable declaration
    let x = 10
    let y = 20
    let z = x + y
    printfn "%i + %i = %i" x y z

    // variables are immutable, so any attempt to reassign a variable results in an error message

let var2() =
    // Variable definition with type declaration
    let x:int32 = 10
    let y:int32 = 20
    let z:int32 = x + y
    printfn "%d + %d = %d" x y z

let var3() =
    // Mutable variables
    let mutable x = 10
    let y = 20
    let mutable z = x + y
    printfn "%i + %i = %i" x y z

    x <-15
    z <- x + y
    printfn "%i + %i = %i" x y z




let main() =
    Console.WriteLine("\nF# VARIABLES\n")

    var1()
    var2()
    var3()


    




