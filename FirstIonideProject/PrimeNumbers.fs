module PrimeNumbers

open System.Collections.Generic


/// https://martin000.wordpress.com/2009/04/05/hello-world/


let allprimes =
    let sieve snums prime =
        seq {for i in snums do if i % prime <> 0 then yield i}

    let rec allprimesrec nums = seq {
        let prime = Seq.head nums   //Seq.hd nums
        yield prime
        yield! allprimesrec (sieve nums prime)
        }
    allprimesrec {2..System.Int32.MaxValue}
    

// This modified version of the allprimes function is MUCH faster because the
// GetEnumerator() call (which happens implicitly in the first version in the
// for loop) has been moved outside the sequence expression. Therefore, when the
// sieve is called a second time, the Enumerator is not recreated.
let allprimes2 =
    let sieve (enum: IEnumerator<int>) =
        let prime = enum.Current
        seq {
        while enum.MoveNext() do
            if enum.Current % prime <> 0
            then yield enum.Current
            }

    let rec allprimesrec (nums: int seq) = seq {
        let enum = nums.GetEnumerator()
        if enum.MoveNext() then
            yield enum.Current
            yield! allprimesrec (sieve enum)
        }
    allprimesrec {2..System.Int32.MaxValue}

    
// A not-so-functional (but very fast) prime generator
let isprime =
    let testPrimes = new ResizeArray<int>()
    testPrimes.Add(2); testPrimes.Add(3)
    let checkprime nn =
        let mutable i = 0
        let mutable maybeprime = true
        let mutable notyetsure = true
        while notyetsure do
            let d = testPrimes.[i]
            let q = nn / d
            if q < d then notyetsure <- false
            elif d*q=nn then maybeprime <-false; notyetsure <-false
            else i <- i+1
        maybeprime
    let updateTestPrimes nn =
        let mutable c = testPrimes.[testPrimes.Count-1]
        if c*c <= nn then
            let mutable needmore = true
            while needmore do
                c <- c+2
                if checkprime c then
                    testPrimes.Add c
                    if c*c > nn then needmore <- false
    let isprimeFunc nn =
        updateTestPrimes nn
        checkprime nn
    isprimeFunc

// using the above
// Computes 2000000th prime (32452843) in about 4 seconds
let nthPrime nth =
    if nth < 1 then failwith "negative nth"
    else if nth = 1 then 2
    else if nth = 2 then 3
    else
        let mutable i = 3
        let mutable c = 3
        while i <= nth do
            c <-c + 2
            if isprime c then i <- i+1
        c

// fast version, more functional
let allprimes7 =
    let testPrimes = new ResizeArray<int>()
    testPrimes.Add(2); testPrimes.Add(3)
    let rec checkprime nn i =
        let d = testPrimes.[i]
        let q = nn / d
        if d*q = nn then false
        elif q < d then true
        else checkprime nn (i+1)
    let rec updateTP c =
        if checkprime c 0 then testPrimes.Add c
        else updateTP (c+2)
    let updateTestPrimes nn =
        let c = testPrimes.[testPrimes.Count-1]
        if c*c <= nn then updateTP (c+2)
    let isprime nn =
        updateTestPrimes nn
        checkprime nn 0
    seq { yield 2; for i in 3..2..System.Int32.MaxValue do if isprime i then yield i }

// even more functional version
let rec allprimes9 =
    let cacheUpToSqrt (s: int seq) =
        let c = new ResizeArray<int>([|3|])
        let e = s.GetEnumerator()
        let cache nn =
            let m = c.[c.Count-1]
            if m*m <= nn then e.MoveNext(); c.Add e.Current
            c
        cache
    seq {
    yield 2; yield 3
    let cache = cacheUpToSqrt allprimes9
    let isprime n = cache n |> Seq.forall (fun i -> (n%i <> 0))
    yield! { 5..2..System.Int32.MaxValue} |> Seq.filter isprime
    }
        
// slower but VERY short version
let rec allprimes10 =
    seq {
    yield 2
    let cache = allprimes10 |> Seq.cache
    let isprime n =
        cache |> Seq.takeWhile (fun i -> i*i <= n) |> Seq.forall (fun i -> n%i <> 0)
    yield! { 3..2..System.Int32.MaxValue} |> Seq.filter isprime
    }







///////////////////////////////////////////////////////////////////////////////

let demo() =

    // For these functions, allprimes took 0.937ms and allprimes2 took 0.015ms
    // (so about about 63x as fast!!!)
    let n = 2000000

    // this one is SO slow!!!
    printfn "allprimes   Compute %ith prime number: %i" n (allprimes |> Seq.item n)
    
    // faster (and more functional in design) but stack overflow error with large numbers?
    printfn "allprimes2  Compute %ith prime number: %i" n (allprimes2 |> Seq.item n)
    

    // these are much faster (and more functional in design)
    // FASTEST!!!
    printfn "allprimes7  Compute %ith prime number: %i" n (allprimes7 |> Seq.item n)
    

    // 2nd FASTEST
    printfn "allprimes9  Compute %ith prime number: %i" n (allprimes9 |> Seq.item n)
    
    // 3rd FASTEST
    printfn "allprimes10 Compute %ith prime number: %i" n (allprimes10 |> Seq.item n)

    // Some sample run times (seconds) for 2,000,000:
    // allprimes7 :    2.76
    // allprimes9 :   15.06
    // allprimes10: 1:30.78


    // Runs forever...
    //for i in allprimes do printfn "%d" i











