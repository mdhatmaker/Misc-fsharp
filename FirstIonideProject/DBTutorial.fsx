open System


module OtherStuff =
    /// Dictionaries with dict, .Keys and .Values
    let table = [ 'C',3;'O',15;'L',12;'I',9;'N',14 ]
    let dictionary = dict table
    let Value c =
        match dictionary.TryGetValue(c) with
        | true, v -> v
        | _ -> failwith (sprintf "letter '%c' was not in lookup table" c)
    let CalcValue name =
        name |> Seq.sumBy Value
    printfn "COLIN = %d" (CalcValue "COLIN")
    printfn "Dictionary Length: %d" dictionary.Count
    for pair in dictionary do
        printfn "%A" pair
    printfn "%A" (dictionary.Item('L'))
    let keys = dictionary.Keys
    Seq.iter (fun k -> printfn "Key = %A" k) keys
    Seq.iter (fun v -> printfn "Value = %A" v) dictionary.Values

module DoFuncs =
    let run_it() =
        let rand_list = [1;2;3]
        let rand_list2 = List.map (fun x -> x * 2) rand_list
        printfn "Double List: %A" rand_list2
      
        [5;6;7;8]
        |> List.filter (fun v -> (v % 2) = 0)
        |> List.map (fun x -> x * 2)
        |> printfn "Even Doubles: %A"

        let mult_num x = x * 3
        let add_num y = y + 5
        let mult_add = mult_num >> add_num
        let add_mult = mult_num << add_num
        printfn "mult_add = %d" (mult_add 2)
        printfn "add_mult = %d" (add_mult 2)

module DoMath =
    let run_it() =
        printfn "5 + 4 = %i" (5 + 4)
        printfn "5 * 4 = %i" (5 * 4)
        printfn "5 / 4 = %i" (5 / 4)
        printfn "5 %% 4 = %i" (5 % 4)
        printfn "5 ** 2 = %.1f" (5.0 ** 2.0)

        let number = 2
        printfn "Type : %A" (number.GetType())
        printfn "A float : %.2f" (float number)
        printfn "An int : %i" (int 3.14159)
        printfn "An int : %i" (int 3.9)     // truncating, not rounding

module StringStuff =
    let run_it() =
        let chrFromA i =
            let ascii = i + (int 'A')
            (char ascii)
        let str1 = "This is a random string"
        let str2 = @"I ignore backslashes"
        let str3 = """Now I can "quote" words in this string"""
        let str4 = str1 + " " + str2
        printfn "Length : %i" (String.length str4)
        printfn "%c" str1.[1]
        printfn "1st word : %s" (str1.[0..3])
        let commas_str = "commas"
        let upper_str = String.collect (fun c -> sprintf "%c, " c) commas_str
        printfn "Commas: %s" upper_str
        printfn "Any upper in '%s': %b" str1 (String.exists (fun c -> Char.IsUpper(c)) str1)
        printfn "Any upper in '%s': %b" commas_str (String.exists (fun c -> Char.IsUpper(c)) commas_str)
        printfn "Is Number: %b" (String.forall (fun c -> Char.IsDigit(c)) "1234")
        let string1 = String.init 10 (fun i -> i.ToString())
        let string2 = String.init 5 (fun i -> (int i).ToString())
        printfn "%s %s" string1 string2
        let string3 = String.init 5 (fun i -> (i+(int 'A')).ToString())
        let string4 = String.init 5 (fun i -> (i+(int 'A')).ToString("0 "))
        let string5 = String.init 5 (fun i -> (i+(int 'A')).ToString("000 "))
        let string6 = String.init 5 (fun i -> sprintf "%c" (chrFromA i))
        printfn "Numbers: %s    %s    %s    %s" string3 string4 string5 string6
        String.iter (fun c -> printfn "%c" c) "Print Me"
        String.collect (fun c -> sprintf "%c " c) "Print Me"
        
module LoopStuff =
    (*let magic_num = "7"
    let mutable guess = ""
    while not (magic_num.Equals(guess)) do
        printf "Guess the number: "
        guess <- Console.ReadLine()
    printfn "You guessed the number!"*)
    /// For loops
    for i = 1 to 10 do
        printf "%i " i
    printfn ""
    for i = 10 downto 1 do
        printf "%i " i
    printfn ""
    for i in  [1..10] do
        printf "%i " i
    printfn ""
    [100..110] |> List.iter (printf "%i ")
    printfn ""
    let sum = List.reduce (+) [1..10]
    printfn "Sum: %i" sum

module CondStuff =
    let age = 8
    if age < 5 then
        printfn "Preschool"
    elif age = 5 then
        printfn "Kindergarten"
    elif (age > 5) && (age <= 18) then
        let grade = age - 5
        printfn "Go to Grade %i" grade
    else
        printfn "Go to College"

    let grade2: string =
        match age with
        | age when age < 5 -> "Preschool"
        | 5 -> "Kindergarten"
        | age when (age > 5 && age <= 18) -> (age - 5).ToString()
        | _ -> "College"
    printfn "Grade2: %s" grade2

module ListStuff =
    let list1 = [1;2;3;4]
    list1 |> List.iter (printfn "Num: %i")
    printfn "%A" list1
    let list2 = 5::6::7::[]
    printfn "%A" list2
    let list3 = [1..5]
    let list4 = ['a'..'g']
    printfn "%A" list4
    let list5 = List.init 5 (fun i -> i * 2)
    printfn "%A" list5
    let list6 = [ for a in 1..5 do yield a*a ]
    let list7 = [ for a in 1..20 do if a%2=0 then yield a ]
    let list8 = [ for a in 1..3 do yield! [a .. a+2] ]
    printfn "%A\n%A\n%A" list6 list7 list8
    printfn "Length: %i" list8.Length
    printfn "Empty: %b" list8.IsEmpty
    printfn "Index 2: %c" (list4.Item(2))
    printfn "Head: %c" (list4.Head)
    printfn "Tail: %A" (list4.Tail)
    let list9 = list3 |> List.filter (fun x -> x % 2 = 0)
    let list10 = list9 |> List.map (fun x -> (x * x))
    printfn "Sorted: %A" (List.sort [5;4;3])
    printfn "Sum: %i" (List.fold (fun partialSum elem -> partialSum + elem) 0 [1;2;3])
    

type emotion =
| joy = 0
| fear = 1
| anger = 2

module EnumStuff =
    
    let show_emotion my_feeling =
        match my_feeling with
        | emotion.joy -> printfn "I'm joyful"
        | emotion.fear -> printfn "I'm fearful"
        | _ -> printfn "Some other emotion"
    show_emotion emotion.joy
    show_emotion emotion.anger


module OptionStuff =
    let divide x y =
        match y with
        | 0 -> None
        | _ -> Some (x/y)
    let showDivide x y =
        let div = (divide x y)
        if div.IsSome then
            printfn "result = %A" (div.Value)
        elif div.IsNone then
            printfn "Can't divide by zero"
        else
            printfn "Something happened" 
    showDivide 5 0
    showDivide 5 1
    showDivide 5 2    

module TupleStuff =
    let avg (w, x, y, z) : float =
        let sum = w + x + y + z
        sum / 4.0
    printfn "Avg: %f" (avg (1.0, 2.0, 3.0, 4.0))
    let my_data = ("Derek", 42, 6.25)
    let (name1, _, _) = my_data
    printfn "Name: %s" name1
    let (name2, age, _) = my_data
    printfn "Name: %s  Ago: %d" name2 age


type customer =
    { Name: string;
    Balance: float }

module RecordStuff =
    let bob = { Name="Bob Smith"; Balance=101.50 }
    printfn "%s owes us %.2f" bob.Name bob.Balance


module SeqStuff =
    // A sequence is not generated until it is required
    let seq1 = seq { 1..100 }
    let seq2 = seq { 0..2..50 }
    let seq3 = seq { 50..1 }
    printfn "%A" seq2
    Seq.toList seq2 |> List.iter (printf "%i, ")
    printfn ""
    let is_prime n =
        let rec check i =
            i > n/2 || (n % i <> 0 && check (i+1)) 
        check 2
    let prime_seq = seq { for n in 1..500 do if is_prime n then yield n }
    printfn "%A" prime_seq
    Seq.toList prime_seq |> List.iter (printfn "Prime: %i")

module MapStuff =
    let customers = 
        Map.empty.
            Add("Bob Smith", 100.50).
            Add("Sally Marks", 50.25)
    printfn "# of customers: %i" customers.Count
    let cust = customers.TryFind "Bob Smith"
    match cust with
    | Some x -> printfn "Balance: %.2f" x
    | None -> printfn "Not Found"
    printfn "Customers: %A" customers
    if customers.ContainsKey "Bob Smith" then
        printfn "Bob Smith was found"
    printfn "Bobs Balance: %.2f" customers.["Bob Smith"]
    let custs2 = Map.remove "Sally Marks" customers
    printfn "# of customers: %i" custs2.Count


let add_stuff<'T> x y =
    printfn "%A" (x + y)

module GenericStuff =
    //add_stuff<float> 5.5 2.4
    add_stuff<int> 5 2


module ExceptionStuff =
    let dividef x y =
        try
            printfn "%.2f / %.2f = %.2f" x y (x / y)
        with
            | :? System.DivideByZeroException -> printfn "Can't divide by zero!"
    dividef 5.0 4.0
    dividef 5.0 0.0
    let dividef2 x y =
        try
            if y = 0.0 then raise(DivideByZeroException "Can't divide by 0")
            else
                printfn "%.2f / %.2f = %.2f" x y (x / y)
        with
            | :? System.DivideByZeroException -> printfn "Can't divide by zero!"
    dividef2 5.0 0.0
    let dividei x y =
        try
            printfn "%i / %i = %i" x y (x / y)
        with
            | :? System.DivideByZeroException -> printfn "Can't divide by zero!"
    dividei 5 0        


type Rectangle = struct
    val Length: float
    val Width: float
    new (length, width) =
        { Length = length; Width = width }
end

module StructStuff =
    let area (shape:Rectangle) =
        shape.Length * shape.Width
    let rect = new Rectangle (5.0, 6.0)
    let rect_area = area rect
    printfn "Area: %A" rect_area


type Animal = class
    val Name: string
    val Height: float
    val Weight: float
    new (name, height, weight) =
        { Name=name; Height=height; Weight=weight }
    member x.Run =
        printfn "%s Runs" x.Name
end

type Dog(name, height, weight) =
    inherit Animal (name, height, weight)
    member x.Bark =
        printfn "%s Barks" x.Name

module ClassStuff =
    let jeffrey = new Animal ("Giraffe", 30.0, 1000.0)
    do jeffrey.Run
    let spot = new Dog ("Spot", 20.5, 40.5)
    do spot.Run
    do spot.Bark
    //do jeffrey.Bark     // ERROR: not defined for base class Animal

/////////////////////////////////////////////////////////////////////

module MoreStuff =
    let prefix prefixStr baseStr =
        prefixStr + ", " + baseStr
    prefix "Hello" "David"
    let prefixHello =
        prefix "Hello"
    prefixHello "Michael"
    let exclaim str =
        str + "!"
    let names = ["David";"Michael";"Fred";"Amy"]
    names
    |> Seq.map prefixHello
    |> Seq.map exclaim
    |> Seq.sort
    |> Seq.iter (printfn "%s")

//StringStuff.run_it()
printfn "======================================================="
