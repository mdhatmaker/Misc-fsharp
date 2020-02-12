module tp3

// https://www.tutorialspoint.com/fsharp/fsharp_quick_guide.htm

open System


(*
  ARITHMETIC OPERATORS
+  -  *  /  %  **

  COMPARISON OPERATORS
=  <>  >  <  >=  <=

  BOOLEAN OPERATORS
&&  ||  not

  BITWISE OPERATORS
&&&   |||   ^^^         binary AND, OR, XOR
~~~   <<<   >>>         binary ONES-COMPLEMENT, LEFT SHIFT, RIGHT SHIFT
*)
let ops1() =
    let A = 60  // 0011 1100
    let B = 13  // 0000 1101
    let c = A &&& B // 0000 1100
    let d = A ||| B // 0011 1101
    let e = ~~~A    // 1100 0011
    printfn "%x %x   &&& = %x  ||| = %x  ~~~ = %x" A B c d e
    



let main() =
    Console.WriteLine("\nF# OPERATORS\n")

    ops1()




    




