module StringStuff



let toPigLatin (word: string) =
    let isVowel (c: char) =
        match c with
        | 'a' | 'e' | 'i' | 'o' | 'u'
        | 'A' | 'E' | 'I' | 'O' | 'U' -> true
        |_ -> false

    if isVowel word.[0] then
        word + "yay"
    else
        word.[1..] + string(word.[0]) + "ay"

let generatePigLatin argv =
    printfn "F# Pig Latin Generator"

    //printfn "Type of argv: %s" (argv.GetType().Name)
    //let argv = ["banana"; "apple"; "kiwi"; "grape"]

    for name in argv do
        let newName = toPigLatin name
        printfn "%s in Pig Latin is: %s" name newName



