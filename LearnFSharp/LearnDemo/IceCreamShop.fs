module LearnDemo.IceCreamShop

open System


//
// ---------- Ice Cream Shop ----------
//

let icecreamShop = "Klaras Parlor"

//let resultOfDay numberOfScoopsSold : float =
let resultOfDay numberOfScoopsSold =
    numberOfScoopsSold * 0.9

let dayResults sold price : float =
    sold * price

let priceFor (flavor: string) =
    if flavor.ToLower() = "strawberry" then
        1.1
    else
        0.9

dayResults 50. (priceFor "Strawberry")

priceFor "Strawberry"
|> dayResults 50.

let strawberryResult =
    "Strawberry"
    |> priceFor
    |> dayResults 50.

let vanillaResult =
    "Vanilla"
    |> priceFor
    |> dayResults 100.

strawberryResult + vanillaResult

let resultsFor ice sold =
    ice
    |> priceFor
    |> dayResults sold

resultsFor "Strawberry" 100. + resultsFor "Vanilla" 50.

let iceFor (flavor: string) =
    if flavor.ToLower() = "red rising" then
        "Strawberry"
    else
        "Vanilla"

"Red Rising"
|> iceFor
|> priceFor
|> dayResults 50.

let priceForSpecial flavor =
    flavor
    |> iceFor
    |> priceFor

"Red Rising"
|> priceForSpecial
|> dayResults 50.

let priceForSpecialComp =
    iceFor >> priceFor

"Red Rising"
|> priceForSpecialComp
|> dayResults 50.


/// LISTS


let sales =
    [
        "Red Rising"
        "Red Rising"
        "Cream Dream"
        "Red Rising"
        "Cream Dream"
    ]

// map each element of the list to a price
// sum all the elements in the list

sales
|> List.map (fun flavor -> priceForSpecial flavor)
|> List.sum

sales
|> List.map priceForSpecial
|> List.sum

sales
|> List.map priceForSpecial
|> List.reduce (+)

sales
|> List.filter (fun flavor -> flavor = "Red Rising")
|> List.map priceForSpecial
|> List.sum

sales
|> List.filter (fun flavor -> flavor = "Red Rising")
|> List.map priceForSpecial
|> List.map (fun x -> x + 0.2)
|> List.sum

sales
|> List.filter (fun flavor -> flavor = "Cream Dream")
|> List.length


(*
/// DISCRIMINATED UNIONS AND PATTERN MATCHING

type SpecialFlavor =
    | RedRising
    | CreamDream

type Flavor =
    | Strawberry
    | Vanilla

let flavor = Vanilla

let specialFlavor = RedRising

let sales2 =
    [
        RedRising
        RedRising
        CreamDream
        RedRising
        CreamDream
        RedRising
    ]

let iceFor2 ice =
    match ice with
    | RedRising ->
        Strawberry
    | CreamDream ->
        Vanilla

let priceFor2 (flavor: Flavor) =
    match flavor with
    | Strawberry ->
        1.1
    | Vanilla ->
        0.9
   
let priceForSpecial2 =
    iceFor2 >> priceFor2

sales2
|> List.map priceForSpecial2
|> List.sum
*)


/// MORE DISCRIMINATED UNIONS AND RECURSIVE FUNCTIONS

type SpecialFlavor =
    | RedRising
    | CreamDream

type Flavor =
    | Strawberry
    | Vanilla
    | Special of SpecialFlavor

let sales3 =
    [
        Special RedRising
        Special RedRising
        Special RedRising
        Special CreamDream
        Special RedRising
        Special CreamDream
        Strawberry
    ]

let rec priceFor2 flavor =
    match flavor with
    | Strawberry ->
        1.1
    | Vanilla ->
        0.9
    (*| Special special ->
        match special with
        | RedRising ->
            (priceFor2 Strawberry) + 0.2
        | CreamDream ->
            (priceFor2 Vanilla) + 0.2*)
    | Special RedRising ->
        (priceFor2 Strawberry) + 0.2
    | Special CreamDream ->
        (priceFor2 Vanilla) + 0.2

sales3
|> List.map priceFor2
|> List.sum


/// RECORDS

type Size =
    | Small
    | Medium
    | Large

type Weekday =
    | Sunday
    | Monday
    | Tuesday
    | Wednesday
    | Thursday
    | Friday
    | Saturday

type IceSold =
    {
        Flavor : Flavor
        Size : Size
        Weekday : Weekday
    }

type OnSale =
    {
        Flavor : Flavor
        Weekday : Weekday
        Factor : float
    }

let onSale weekday flavor =
    {
        Flavor = flavor
        Weekday = weekday
        Factor = 0.5
    }

let sold weekday flavor =
    {
        Flavor = flavor
        Size = Medium
        Weekday = weekday
    }

let onSalePerWeek1 =
    [
        Strawberry |> onSale Monday
        Special RedRising |> onSale Monday
        Special CreamDream |> onSale Wednesday
        Special RedRising |> onSale Wednesday
    ]

let onSalePerWeek2 =
    [
        Strawberry |> onSale Wednesday
        Special CreamDream |> onSale Monday
        Special RedRising |> onSale Thursday
    ]

//let vanilla =
//    sold Vanilla
//{ vanilla with Size = Large }

let updateSize iceSold size =
    { iceSold with Size = size }


let withSize size iceSold =
    { iceSold with Size = size }

//updateSize vanilla Small

//updateSize (sold Strawberry) Large

//Strawberry
//|> sold Monday
//|> withSize Large



let sales4 =
    [
        Special RedRising |> sold Monday |> withSize Large
        Special RedRising |> sold Monday |> withSize Small
        Special CreamDream |> sold Monday |> withSize Medium
        Special RedRising |> sold Wednesday |> withSize Small
        Special CreamDream |> sold Wednesday |> withSize Large
        Strawberry |> sold Wednesday |> withSize Medium
    ]

let multiplierFor size =
    match size with
    | Small ->
        0.7
    | Medium ->
        1.0
    | Large ->
        1.3

let totalResult (iceSold: IceSold) =
    (iceSold.Flavor |> priceFor2) * (iceSold.Size |> multiplierFor)


sales4
|> List.map totalResult
|> List.sum


/// CURRYING

let totalPrice2 (onSales : OnSale list) (iceSold : IceSold) =
    let onSale =
        onSales |> List.find (fun onSale -> onSale.Flavor = iceSold.Flavor && onSale.Weekday = iceSold.Weekday) 
    priceFor2 iceSold.Flavor * multiplierFor iceSold.Size * onSale.Factor

sales4
|> List.map (totalPrice2 onSalePerWeek1)
|> List.sum

let onMonday =
    sold Monday

let onTuesday =
    sold Tuesday

let onWednesday =
    sold Wednesday

onMonday Strawberry
onTuesday Strawberry
onWednesday Strawberry


let sales5 =
    [
        Special RedRising |> onMonday |> withSize Large
        Special RedRising |> onMonday |> withSize Small
        Special CreamDream |> onMonday |> withSize Medium
        Special RedRising |> onWednesday |> withSize Small
        Special CreamDream |> onWednesday |> withSize Large
        Strawberry |> onWednesday |> withSize Medium
    ]

sales4
|> List.map (totalPrice2 onSalePerWeek2)
|> List.sum

sales5
|> List.map (totalPrice2 onSalePerWeek1)
|> List.sum
