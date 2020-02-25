module ParallelStuff


/// https://www.itu.dk/~sestoft/bachelor/fsharp-async-parallel-2016.pdf
/// http://tomasp.net/blog/fsharp-parallel-samples.aspx/
/// https://archive.codeplex.com/?p=fsharppowerpack
/// https://fsprojects.github.io/FSharp.Collections.ParallelSeq/


open System
open System.Diagnostics
open System.Net

//#r "FSharp.Collections.ParallelSeq.dll"
//#r @"C:\Users\mhatm\.nuget\packages\fsharp.collections.parallelseq\1.1.2\lib\netstandard2.0\FSharp.Collections.ParallelSeq.dll"
open FSharp.Collections.ParallelSeq

open PrimeNumbers


///////////////////////////////////////////////////////////////////////////////
/// Cleaner method to time the execution of a function (passed as arg)
let duration f = 
    let timer = new System.Diagnostics.Stopwatch()
    timer.Start()
    let returnValue = f()
    printfn "Elapsed Time: %i" timer.ElapsedMilliseconds
    returnValue   

let duration_demo =
    let sample() = System.Threading.Thread.Sleep(2)
    duration ( fun() -> sample() )
    // or
    duration sample

///////////////////////////////////////////////////////////////////////////////
/// https://www.cs.hmc.edu/~oneill/papers/Sieve-JFP.pdf
let rec sieve = function
    | (p::xs) -> p :: sieve [ for x in xs do if x % p > 0 then yield x ]
    | [] -> []

let primes = sieve [2..5000]
printfn "%A" primes     // [2;3;5;7;11;13;17;19;23;29;31;37;41;43;47...]


/////////////////////////////////////////////////////////////////////
let main2() =
    System.Console.WriteLine("F# ParallelSeq Library")

    let nums = [|1..500000|]
    let finalDigitOfPrimes =
        nums
        |> PSeq.filter isprime
        |> PSeq.groupBy (fun i -> i % 10)
        |> PSeq.map (fun (k, vs) -> (k, Seq.length vs))
        |> PSeq.toArray

    printfn "=================================================================="

///////////////////////////////////////////////////////////////////////////////
let main1() =
    Console.WriteLine("Basic Parallel Task Samples\n")
    (*// Run several samples and measure the time taken
    SampleUtilities.TimedAction "2 steps, sequential" (fun () ->
        Chapter3Sample01Sequential() )
    SampleUtilities.Timedaction "2 steps, parallel invoke" (fun () ->
        Champter3Sample01ParallelInvoke() )
    // (Other samples omitted) *)

   
    let timeFunctionX (iterCount:int) (desc:string) (f: unit->unit) =
        printfn "===== Starting timed function '%s'  (%i iterations) =================================================" desc iterCount
        let timer1 = System.Diagnostics.Stopwatch.StartNew()
        for i in 1..iterCount do
            f()
        timer1.Stop()
        let milliseconds = timer1.Elapsed.TotalMilliseconds
        let avg = milliseconds / (float iterCount)
        printfn "===== Average time elapsed for function '%s' is %s ms  (%.0f total elapsed) =====================" desc (avg.ToString("#,##0")) milliseconds
    let timeFunction1 = timeFunctionX 1
    let timeFunction5 = timeFunctionX 5


    let timeFunction = timeFunctionX 3


    let rec slowfib n =
        if n<2 then 1.0 else slowfib(n-1) + slowfib(n-2)
    
    

    let f1 = (fun () ->
        let fib42 = slowfib 42
        () )
    timeFunction "f1" (f1)


    let f2 = (fun () ->
        let fibs = [ slowfib(41); slowfib(42) ]
        () )
    do timeFunction "f2" (f2)


    let f2a = (fun () ->
        let tasks = [ async { return slowfib(41) };
                      async { return slowfib(42) } ]
        Async.RunSynchronously (Async.Parallel tasks)
        () )
    do timeFunction "f2a" (f2a)


    let f3 = (fun () ->
        [ for i in 0..42 do yield slowfib(i) ]
        () )
    do timeFunction "f3" (f3)


    let f3a = (fun () ->
        let tasks =
            [ for i in 0..42 do yield async { return slowfib(i) } ]
        Async.RunSynchronously (Async.Parallel tasks)
        () )
    do timeFunction "f3a" (f3a)

    printfn "=================================================================="

///////////////////////////////////////////////////////////////////////////////


let main3() =
    
    let urls = ["http://www.itu.dk"; "http://www.diku.dk";
                "http://www.google.com"; "http://www.youtube.com"; "http://www.tmall.com";
                "http://www.facebook.com"; "http://www.baidu.com"; "http://www.qq.com"; "http://www.sohu.com";
                "http://www.taobao.com"; "http://www.yahoo.com"; "http://360.cn"]

    let lengthSync (url: string) =
        let wc = new WebClient()
        let html = wc.DownloadString(Uri(url))
        html.Length

    // visit a single URL
    let url = "http://www.diku.dk"
    printfn "lengthSync: %i" (lengthSync(url))

    // visit each URL in the urls list 
    [ for url in urls do yield lengthSync url ]

    
    /// Now the parallel version (even with 1 CPU, faster because it's I/O-bound
    let lens =
        let tasks = [ for url in urls do yield async { return lengthSync url } ]
        Async.RunSynchronously(Async.Parallel tasks)

    // Better: Let I/O system deal with responses
    let lengthAsync2 (url: string) =
        async {
            printf ">>>%s>>>\n" url
            let wc = new WebClient()
            let! html = wc.AsyncDownloadString(Uri(url))
            printf "<<<%s<<<\n" url
            return html.Length
        }

    // NOTE: wc.AsyncDownloadString(...) is asynchronous, and we can have many
    // more active requests than there are threads (500 threads is bad, but
    // 50,000 async concurrent requests is fine).

    let lens2 =
        let tasks = [ for url in urls do yield lengthAsync2 url ]
        Async.RunSynchronously(Async.Parallel tasks)

    printfn "=================================================================="

    




