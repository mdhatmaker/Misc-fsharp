


open System


/// Compute the digits of pi
/// https://cs.uwaterloo.ca/~alopez-o/math-faq/mathtext/node12.html
/// http://pi.fathom.info/
/// http://www.codecodex.com/wiki/Calculate_digits_of_pi#Python




/// https://fsharpforfunandprofit.com/posts/computation-expressions-intro/

let add1 x = x + 1

let plus1 = add1
add1 5
plus1 5

let intToString x = sprintf "x is %i" x
let stringToInt x = System.Int32.Parse(x)
intToString 5
stringToInt "10"

let intToBool x = (x <> 0)
intToBool 1
intToBool 0

let stringLength (x:string) = x.Length

let evalWith5ThenAdd2 fn = fn 5 + 2

let adderGenerator numberToAdd = (+) numberToAdd
let f1 = adderGenerator 5
f1 6

let printTwoParameters x y = printfn "x=%i y=%i" x y

let add42 = (+) 42
[1;2;3] |> List.map add42;;

let twoIsLessThan = (<) 2
twoIsLessThan 1
twoIsLessThan 3
[1;2;3] |> List.filter twoIsLessThan

let printer = printfn "printing param=%i"
[1;2;3] |> List.iter printer

let add1 = (+) 1
let add1ToEach = List.map add1
add1ToEach [1;2;3;4]

let filterEvens = List.filter (fun i -> i%2 = 0)
filterEvens [1;2;3;4]

let adderWithPluggableLogger logger x y =
    logger "x" x
    logger "y" y
    let result = x + y
    logger "x+y" result
    result

let consoleLogger argName argValue =
    printfn "%s=%A" argName argValue

let adder1 = adderWithPluggableLogger consoleLogger
adder1 1 2

let popupLogger argName argValue =
    let message = sprintf "%s=%A" argName argValue
    System.Windows.Forms.MessageBox.Show(
                            text=message,caption="Logger")
        |> ignore

let adder2 = adderWithPluggableLogger popupLogger
adder2 1 2

let adder3 = adder1 42
[1;2;3] |> List.map adder3

List.sortBy (fun i -> -i) [0;1;2;3]
let sortDesc = List.sortBy (fun i -> -i)
sortDesc [0;1;2;3]

List.filter (fun i -> i>1) [0;1;2;3]
let excludeOneOrLess = List.filter (fun i -> i>1)
excludeOneOrLess [0;1;2;3]

let result =
    [1..10]
    |> List.map (fun i -> i+1)
    |> List.filter (fun i -> i>5)

// create wrappers for .NET string functions
let replace oldStr newStr (s:string) =
    s.Replace(oldValue=oldStr, newValue=newStr)

let startsWith lookFor (s:string) =
    s.StartsWith(lookFor)

let result =
    "hello"
    |> replace "h" "j"
    |> startsWith "j"

let compositeOp = replace "h" "j" >> startsWith "j"
let result = compositeOp "hello"

"12" |> int
1 |> (+) 2 |> (*) 3

let f (x:int) = float x * 3.0
let g (x:float) = x > 4.0
let h = f >> g
h 1
h 2

let add1ThenMultiply = (+) 1 >> (*)
add1ThenMultiply 2 7

let myList = []
myList |> List.isEmpty |> not
myList |> (not << List.isEmpty)

let add = fun x y -> x+y
add 3 4

type Name = {first:string; last:string}
let bob = {first="bob"; last="smith"}

// single parameter style
let f1 name =
    let {first=f;last=l} = name
    printfn "OUTPUT: first=%s; last=%s" f l

let f2 {first=f; last=l} =
    printfn "OUTPUT: first=%s; last=%s" f l

/// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/operator-overloading#prefix
let (.+) x y = x + y + 1
5 .+ 6

let ( *+* ) = x + y + 1     // must use spaces so '(*' isn't interpreted as beginning of comment

let (~%%) (s:string) = s.ToCharArray()  // '~' makes operator unary
let result = %% "hello"

let rec fib i =
    match i with
    | 1 -> 1
    | 2 -> 1
    | n -> fib(n-1) + fib(n-2)

type Adder = int -> int
let f1:Adder = fun x -> x + 1

let testA x = x + 1
let testB x y = x + y
let testC x = (+) x


module MathStuff =

    // functions
    let add x y = x + y
    let subtract x y = x - y

    // type definitions
    type Complex = {r:float; i:float}
    type IntegerFunction = int -> int -> int
    type DegreesOrRadians = Deg | Rad

    // "constant"
    let PI = 3.141

    // "variable"
    let mutable TrigType = Deg

    // initialization / static constructor
    do printfn "module initialized"



type System.Int32 with
    member this.IsEven = this % 2 = 0

let i = 20
if i.IsEven then printfn "'%i' is even" i

let arr1 = Array.create 10 5.0
let (arr2:float array) = Array.zeroCreate 15
let (arr3:int array) = Array.zeroCreate 20
let arr4 = Array.init 10 (fun i -> i * i)

printfn "%A" (Array.filter (fun elem -> elem % 2 = 0) [|1..10|])

let stringReverse (s:string) =
    System.String(Array.rev (s.ToCharArray()))

printfn "%A" (stringReverse("!dlrow olleH"))

let twoDimensionalArray = Array2D.init 2 2 (fun i j -> (i+1)*(j+1))

let twoToFive = [2;3;4;5]
let oneToFive = 1::twoToFive        // :: creates list with new 1st element
let zeroToFive = [0;1] @ twoToFive  // @ concats two lists

let evens list = 
    let isEven x = x%2=0
    List.filter isEven list

let square x = x*x
let sumOfSquares list =
    List.sum (List.map square list)
let sumOfSquaresTo100 = sumOfSquares [1..100]

let validValue = Some(99)
let invalidValue = None

let optionPatternMatch input =
    match input with
    | Some i -> printfn "input is an int=%d" i
    | None -> printfn "input is missing"

type Temp =
    | DegreesC of float
    | DegreesF of float

let temp = DegreesF 98.6

type Person = {First:string; Last:string}
let person1 = {First="bob"; Last="dole"}
type Employee =
    | Worker of Person
    | Manager of Employee list
let jdoe = {First="john"; Last="doe"}
let worker = Worker jdoe

/// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/arrays
let compare y =
    let delta = 1.0e-10
    abs(y - round y) < delta

let arrayA = [|2..100|]
//let delta = 1.0e-10
let isPerfectSquare (x:int) =
    //let y = sqrt (float x)
    //abs(y - round y) < delta
    compare (sqrt (float x))
let isPerfectCube (x:int) =
    //let y = System.Math.Pow(float x, 1.0/3.0)
    //abs(y - round y) < delta
    compare (System.Math.Pow(float x, 1.0/3.0)) // cube root
let element = Array.find (fun elem -> isPerfectSquare elem && isPerfectCube elem) arrayA
let index = Array.findIndex (fun elem -> isPerfectSquare elem && isPerfectCube elem) arrayA
printfn "The first element that is both a square and a cube is %d and its index is %d." element index

let array1 = [|1;2;3;|]
let array2 = [|-1;-2;-3|]
let arrayZip = Array.zip array1 array2
Array.unzip arrayZip

let rec fib i =
    match i with
    | 1 -> 1;
    | 2 -> 1;
    | n -> fib(n-1) + fib(n-2)

let arr' = [|1..40|]
let arr = Array.concat [|arr'; arr'; arr'; arr'; arr'; arr'; arr'; arr'|]

let arr1 = Array.map fib arr    // non-parallel

let arr2 = Array.Parallel.map fib arr   // parallel



let rec quicksort2 = function
    | [] -> []
    | first::rest ->
        let smaller,larger = List.partition ((>=) first) rest
        List.concat [quicksort2 smaller; [first]; quicksort2 larger]

// test code
printfn "%A" (quicksort2 [1;5;23;18;9;1;3])

open System
open System.Net
open System.Windows.Forms

let webClient = new WebClient()
let fsharpOrg = webClient.DownloadString(Uri "http://fsharp.org")
let browser =
    new WebBrowser(ScriptErrorsSuppressed=true, Dock=DockStyle.Fill, DocumentText=fsharpOrg)
let form = new Form(Text="Hello from F#!")
form.Controls.Add browser
form.Show()


let r = System.Random()
let generateRandomNumber max =
    let nextValue = r.Next(1, max)
    nextValue

let arr1 = Array.zeroCreate 30
let arr1 = Array.create 30 10
let arr1 = Array.init 30 (fun _ -> generateRandomNumber 10)
Array.map (generateRandomNumber) arr1



/// https://theburningmonk.com/2011/09/fsharp-yield-vs-yield/
let tens = seq { 0..10..100 }

// we can use '->' in place of 'do yield'
let oneToTen = seq { for i in 0..10 -> i }



let withYieldBang =
    seq { for i in 0..10..100 do
            yield! seq { i..1..i+9 } }

let withYield =
    seq { for i in 0..10..100 do
            yield seq { i..1..i+9 } }




// power series expansion of atan(x) = x - x^3/3 + x^5/5 - ... together with formulas
// like pi = 16*atan(1/5) - 4*atan(1/239).

let iseq = seq { 1..4..401 }

let atan x = 
    iseq
    |> Seq.map (fun i -> float i)
    |> Seq.map (fun i' -> x**i'/i')

let atan2 x =
    iseq
    |> Seq.map (fun i -> float (i+1))
    |> Seq.map (fun i' -> x**i'/i')

let res1 x =
    atan x |> Seq.sum
    
let res2 x =
    atan2 x |> Seq.sum

let res x = res1 x - res2 x
    

    


printfn "Atan=%f    atan=%f" (Math.Atan(3.)) (res 3.)


let pi1 = 16. * Math.Atan(1./5.) - 4. * Math.Atan(1./239.)
printfn "pi = %f" pi1


// C program computes pi to 800 decimals:
(*
int a=10000,b,c=2800,d,e,f[2801],g;
main() {
    for ( ; b-c; )
        f[b++] = a/5;
    for ( ; d=0,g=c*2; c-=14,printf("%.4d",e+d/a),e=d%a)
        for( b=c; d += f[b]*a, f[b]=d%--g,d/=g--,--b; d*=b);
}
*)

let SCALE = 10000
let ARRINIT = 2000

let pi_digits (digits: int) =
    let mutable carry = 0
    let mutable arr = Array.init (digits+1) (fun _ -> ARRINIT)
    printfn "%A" arr
    (*for i in 0..digits do
        arr.[i] <- ARRINIT*)
    for i in digits..(-14)..1 do
        let mutable sum = 0
        for j in i..(-1)..0 do
            sum <- sum * j + SCALE * arr.[j]
            arr.[j] <- sum % (j * 2 - 1)
            sum <- sum / (j * 2 - 1)
            printfn "%d %d %d" j arr.[j] sum
        printfn "%d" (carry + sum / SCALE)
        carry <- sum % SCALE
    arr

printfn "%A"(pi_digits 11)


(* C# code to calculate pi digits
int SCALE  = 10000;  
int ARRINIT = 2000;  

void pf(int digits) {
    var str = pid(digits);
    Console.WriteLine("({0}) {1}", str.Length, str);
}

string pid(int digits) {
    return pi_digits((digits/4 + 1) * 14);
}

string pi_digits(int digits) {  
    var sb = new StringBuilder();
    int carry = 0;  
    var arr = new int[digits + 1];  
    for (int i = 0; i <= digits; ++i)  
        arr[i] = ARRINIT;  
    for (int i = digits; i > 0; i-= 14) {  
        int sum = 0;  
        for (int j = i; j > 0; --j) {  
            sum = sum * j + SCALE * arr[j];  
            arr[j] = sum % (j * 2 - 1);  
            sum /= j * 2 - 1;  
        }  
        //Console.WriteLine("%04d", carry + sum / SCALE);  
        //Console.Write("{0:D4}", carry + sum / SCALE);  
        sb.Append(string.Format("{0:D4}", carry + sum / SCALE));
        carry = sum % SCALE;  
    }
    //Console.WriteLine("");
    return sb.ToString();
}  
  
int main(int argc, char** argv) {  
    int n = argc == 2 ? atoi(argv[1]) : 100;  
    pi_digits(n);  
  
    return 0;  
}  
*)

(* Python code to calculate pi digits
from sys import stdout  

scale = 10000  
maxarr = 2800  
arrinit = 2000  
carry = 0  
arr = [arrinit] * (maxarr + 1)  
for i in xrange(maxarr, 1, -14):  
  total = 0  
  for j in xrange(i, 0, -1):  
      total = (total * j) + (scale * arr[j])  
      arr[j] = total % ((j * 2) - 1)  
      total = total / ((j * 2) - 1)  
  stdout.write("%04d" % (carry + (total / scale)))  
  carry = total % scale  
*)

(*
There are essentially 3 different methods to calculate pi to many decimals. 

One of the oldest is to use the power series expansion of atan(x) = x - x^3/3 + x^5/5 - ... together with formulas like pi = 16*atan(1/5) - 4*atan(1/239). This gives about 1.4 decimals per term. 

A second is to use formulas coming from Arithmetic-Geometric mean computations. A beautiful compendium of such formulas is given in the book pi and the AGM, (see references). They have the advantage of converging quadratically, i.e. you double the number of decimals per iteration. For instance, to obtain 1 000 000 decimals, around 20 iterations are sufficient. The disadvantage is that you need FFT type multiplication to get a reasonable speed, and this is not so easy to program. 

A third one comes from the theory of complex multiplication of elliptic curves, and was discovered by S. Ramanujan. This gives a number of beautiful formulas, but the most useful was missed by Ramanujan and discovered by the Chudnovsky's. It is the following (slightly modified for ease of programming): 
Set k_1 = 545140134; k_2 = 13591409; k_3 = 640320; k_4 = 100100025; k_5 = 327843840; k_6 = 53360; 
Then pi = (k_6 sqrt(k_3))/(S), where 

S = sum_(n = 0)^oo (-1)^n ((6n)!(k_2 + nk_1))/(n!^3(3n)!(8k_4k_5)^n)

The great advantages of this formula are that 
1) It converges linearly, but very fast (more than 14 decimal digits per term). 
2) The way it is written, all operations to compute S can be programmed very simply. This is why the constant 8k_4k_5 appearing in the denominator has been written this way instead of 262537412640768000. This is how the Chudnovsky's have computed several billion decimals. 



An interesting new method was recently proposed by David Bailey, Peter Borwein and Simon Plouffe. It can compute the Nth hexadecimal digit of Pi efficiently without the previous N-1 digits. The method is based on the formula: 

pi = sum_(i = 0)^oo (1 16^i) ((4 8i + 1) - (2 8i + 4) - (1 8i + 5) - (1 8i + 6))

in O(N) time and O(log N) space. (See references.) 
The following 160 character C program, written by Dik T. Winter at CWI, computes pi to 800 decimal digits. 

     int a=10000,b,c=2800,d,e,f[2801],g;main(){for(;b-c;)f[b++]=a/5;
     for(;d=0,g=c*2;c-=14,printf("%.4d",e+d/a),e=d%a)for(b=c;d+=f[b]*a,
     f[b]=d%--g,d/=g--,--b;d*=b);}
*)

