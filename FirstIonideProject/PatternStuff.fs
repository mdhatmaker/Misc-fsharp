module PatternStuff


// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/active-patterns


// Active pattern of one choice
// let (|identifier|) [arguments] valueToMatch= expression
//
// Active pattern with multiple choices
// let (|identifier1|identifier2|...|) valueToMatch = expression
//
// Partial active pattern definition
// let (|identifier|_|) [arguments] valueToMatch = expression


let activePattern1() =
    
    let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd

    let TestNumber input =
        match input with
        | Even -> printfn "%d is even" input
        | Odd -> printfn "%d is odd" input

    TestNumber 7
    TestNumber 11
    TestNumber 32


open System.Drawing

let activePattern2() =

    let (|RGB|) (col: System.Drawing.Color) =
        ( col.R, col.G, col.B )

    let (|HSB|) (col: System.Drawing.Color) =
        ( col.GetHue(), col.GetSaturation(), col.GetBrightness() )

    let printRGB (col: System.Drawing.Color) =
        match col with
        | RGB(r, g, b) -> printfn " Red: %d Green: %d Blue: %d" r g b

    let printHSB (col: System.Drawing.Color) =
        match col with
        | HSB(h, s, b) -> printfn " Hue: %f Saturation: %f Brightness: %f" h s b

    let printAll col colorString =
        printfn "%s" colorString
        printRGB col
        printHSB col

    printAll Color.Red "Red"
    printAll Color.Black "Black"
    printAll Color.White "White"
    printAll Color.Gray "Gray"
    printAll Color.BlanchedAlmond "BlanchedAlmond"


/// PARTIAL ACTIVE PATTERNS

let activePattern3() =

    let (|Integer|_|) (str: string) =
        let mutable intvalue = 0
        if System.Int32.TryParse(str, &intvalue) then Some(intvalue)
        else None

    let (|Float|_|) (str: string) =
        let mutable floatvalue = 0.0
        if System.Double.TryParse(str, &floatvalue) then Some(floatvalue)
        else None

    let parseNumeric str =
        match str with
        | Integer i -> printfn "%d : Integer" i
        | Float f -> printfn "%f : Floating point" f
        | _ -> printfn "%s : Not matched." str

    parseNumeric "1.1"
    parseNumeric "0"
    parseNumeric "0.0"
    parseNumeric "10"
    parseNumeric "Something else"










