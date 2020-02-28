module ConvertCSharp



open System.IO
open System.Text.RegularExpressions


let readLines (filePath: string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}



let tryIt() =

    let code = readLines @"..\..\code1.txt"
    

    let checkLine (pattern:string*string) (input:string) =
        Regex.Replace(input, fst pattern, snd pattern)
        


    let p1 = ("^using (.+);$", "open $1")


    code
    |> Seq.take 10
    |> Seq.map (checkLine p1)
    |> Seq.iter (printfn "%s")