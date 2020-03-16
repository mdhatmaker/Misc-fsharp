


open System


/// Compute the digits of pi
/// https://cs.uwaterloo.ca/~alopez-o/math-faq/mathtext/node12.html
/// http://pi.fathom.info/
/// http://www.codecodex.com/wiki/Calculate_digits_of_pi#Python

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

