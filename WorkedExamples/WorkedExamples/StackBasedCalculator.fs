module StackBasedCalculator

/// https://fsharpforfunandprofit.com/posts/stack-based-calculator/
/// http://thinking-forth.sourceforge.net/


type Stack = StackContents of float list


let demo() =
    
        (*let push x aStack =
            let (StackContents contents) = aStack
            let newContents = x::contents
            StackContents newContents*)

        // more concise version by using pattern matching in the function parameter itself:
        // (function signature 'val push : float -> Stack -> Stack' is self-documenting)
        let push x (StackContents contents) =
            StackContents (x::contents)

        let pop (StackContents contents) =
            match contents with
            | top::rest ->
                let newStack = StackContents rest
                (top,newStack)
            | [] ->
                failwith "Stack underflow"

        (*let ADD stack =
            let a,s1 = pop stack
            let b,s2 = pop s1
            push (a+b) s2
            
        let MUL stack =
            let a,s1 = pop stack
            let b,s2 = pop s1
            push (a*b) s2*)

        // refactor to eliminate redundant code
        // (helper function for binary functions)
        let binary mathFn stack =
            let y,stack' = pop stack    // pop the top of the stack
            let x,stack'' = pop stack'  // pop the top of the stack again
            let z = mathFn x y          // do the math
            push z stack''              // push the result alue back on the stack

        let unary f stack =
            let x,stack' = pop stack    // pop the top of the stack
            push (f x) stack'           // push the function value on the stack

        let SHOW stack =
            let x,_ = pop stack
            printfn "The answer is %f" x
            stack   // keep going with the same stack

        let DUP stack =                 // Duplicate the top value on the stack
            let x,_ = pop stack         // get the top of the stack
            push x stack                // push it onto the stack again

        let SWAP stack =                // Swap the top two values
            let x,s = pop stack
            let y,s' = pop s
            push y (push x s')

        //let ADD aStack = binary (fun x y -> x + y) aStack
        //let ADD aStack = binary (+) aStack
        let ADD = binary (+)
        let SUB = binary (-)
        let MUL = binary (*)
        let DIV = binary (/)

        let NEG = unary (fun x -> -x)
        let SQUARE = unary (fun x -> x*x)

        let ONE = push 1.0
        let TWO = push 2.0
        let THREE = push 3.0
        let FOUR = push 4.0
        let FIVE = push 5.0

        let EMPTY = StackContents []

        let START = EMPTY               // Make an obvious starting point

        //-------------------------------------------------------------------------------

        // test
        let newStack = StackContents [1.0;2.0;3.0]
        let (StackContents contents) = newStack     // 'contents' set to float list = [1.0;2.0;3.0]

        // test
        let emptyStack = StackContents []
        let stackWith1 = push 1.0 emptyStack
        let stackWith2 = push 2.0 stackWith1

        // test
        let stackWith1' = ONE EMPTY
        let stackWith2' = TWO stackWith1
        let stackWith3' = THREE stackWith2

        // functions ONE, TWO, THREE, etc. have same signature 'Stack -> Stack', so chain them together
        let result123 = EMPTY |> ONE |> TWO |> THREE
        let result312 = EMPTY |> THREE |> ONE |> TWO

        // test
        let initialStack = EMPTY |> ONE |> TWO
        let popped1, poppedStack = pop initialStack
        let popped2, poppedStack2 = pop poppedStack
        
        //let _ = pop EMPTY   // test 'stack underflow' exception
        
        // test
        let add1and2 = EMPTY |> ONE |> TWO |> ADD
        let add2and3 = EMPTY |> TWO |> THREE |> ADD
        let mult2and3 = EMPTY |> TWO |> THREE |> MUL

        // test
        let threeDivTwo = EMPTY |> THREE |> TWO |> DIV
        let twoSubtractFive = EMPTY |> TWO |> FIVE |> SUB
        let oneAddTwoSubThree = EMPTY |> ONE |> TWO |> ADD |> THREE |> SUB

        // test
        let neg3 = EMPTY |> THREE |> NEG
        let square2 = EMPTY |> TWO |> SQUARE

        // test
        EMPTY |> ONE |> THREE |> ADD |> TWO |> MUL |> SHOW  // (1+3)*2 = 8


        // test
        START
            |> ONE |> TWO |> SHOW

        START
            |> ONE |> TWO |> ADD |> SHOW
            |> THREE |> ADD |> SHOW

        START
            |> THREE |> DUP |> DUP |> MUL |> MUL    // 27

        START
            |> ONE |> TWO |> ADD |> SHOW    // 3
            |> THREE |> MUL |> SHOW         // 9
            |> TWO |> DIV |> SHOW           // 9 div 2 = 4.5



        // because these functions have signature 'Stack -> Stack', we can use '>>' (composition operator)
        let ONE_TWO_ADD =
            ONE >> TWO >> ADD

        // test it
        START |> ONE_TWO_ADD |> SHOW

        let SQUARE' =           // define a new function
            DUP >> MUL

        // test it
        START |> TWO |> SQUARE' |> SHOW

        let CUBE =              // define a new function
            DUP >> DUP >> MUL >> MUL

        // test it
        START |> THREE |> CUBE |> SHOW

        let SUM_NUMBERS_UPTO =  // define a new function
            DUP     // n, n 2 items on stack
            >> ONE  // n, n, 1 3 items on stack
            >> ADD  // n, (n+1) 2 items on stack
            >> MUL  // n(n+1) 1 item on stack
            >> TWO  // n(n+1), 2 2 items on stack
            >> DIV  // n(n+1)/2 1 item on stack

        // test it with sum of numbers up to 9
        START |> THREE |> SQUARE |> SUM_NUMBERS_UPTO |> SHOW    // 45













