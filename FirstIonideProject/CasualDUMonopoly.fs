#if COMPILED
namespace CasualFSharp
#endif

module Monopoly =

    open System

    type Player = 
    | Boot
    | Battleship
    | Iron
    | Dog
    | Car
    // Etc.

    type Color =
    | Brown
    | LightBlue
    | Magenta
    | Orange
    | Red
    | Yellow
    // Etc.

    type Development =
    | Empty
    | Houses of count:int
    | Hotel

    type Price = int        // "Type Alias" (not "Distributed Union")

    type Location =
    | Street of
        name:string *
        cost:Price *
        color:Color *
        development:Development *
        rentals:Price[] *
        owner:Player option

    | Utility of
        name:string *
        cost:Price *
        owner:Player option

    | Station of
        name:string *
        cost:Price *
        owner:Player option

    | Tax of name:string * amount:Price


    | GotoJail
    | Go
    | CommunityChest
    | Chance
    | Jail
    | FreeParking


    let ownsLocation player location =
        match location with
        | Street(_, _, _, _, _, Some(owner))
        | Utility(_, _, Some(owner))
        | Station(_, _, Some(owner)) ->
            player = owner
        | _ -> false

    let streetsOfColor color board =
        board
        |> Seq.filter (fun loc ->
            match loc with
            | Street(_, _, c, _, _, _) ->
                color = c
            | _ -> false )

    let ownsStreetSet player color board =
        board
        |> streetsOfColor color
        |> Seq.exists (fun loc ->
            not (ownsLocation player loc))
        |> not

    let charge (player:Player) (location:Location) board =
        match location with
        | Street(_, _, color, development, rentals, owner) ->
            match owner with
            | None ->
                None
            | Some o when o <> player ->
                match development with
                | Empty -> 
                    let factor = if ownsStreetSet o color board then 2 else 1
                    rentals.[0] * factor |> Some
                | Houses c ->
                    rentals.[c] |> Some
                | Hotel ->
                    rentals.[5] |> Some
            | Some _ ->
                None
        | Utility(name, rentals, owner) ->
            raise <| NotImplementedException()
        | _ ->
            raise <| NotImplementedException()





module Demo =
    open Monopoly

    let go = Go
    let oldKentRoad = Street("Old Kent Road", 60, Brown, Empty, [|2;10;20;90;160;250|], None)
    let communityChest = CommunityChest
    let whitechapelRoad = Street("Whitechapel Road", 60, Brown, Empty, [|4; 20; 60; 180; 230; 450|], None)
    let incomeTax = Tax("Income Tax", 200)
    let kingsCross = Station("King's Cross", 200, None)
    let theAngelIslington = Street("The Angel Islington", 100, LightBlue, Empty, [|6; 30; 90; 270; 400; 500|], None)

    let board = [
        go
        oldKentRoad
        communityChest
        whitechapelRoad
        incomeTax
        kingsCross
        theAngelIslington
        // Etc.
    ]

    charge Boot oldKentRoad board |> printfn "Charged: %A"

    let oldKentRoad' = Street("Old Kent Road", 60, Brown, Empty, [|2;10;20;90;160;250|], Some Boot)

    let board' = [
        go
        oldKentRoad'
        communityChest
        whitechapelRoad
        incomeTax
        kingsCross
        theAngelIslington
        // Etc.
    ]

    charge Boot oldKentRoad' board' |> printfn "Charged: %A"
    charge Battleship oldKentRoad' board' |> printfn "Charged: %A"

    let oldKentRoad'' = Street("Old Kent Road", 60, Brown, Houses 3, [|2;10;20;90;160;250|], Some Boot)
    
    let board'' = [
        go
        oldKentRoad''
        communityChest
        whitechapelRoad
        incomeTax
        kingsCross
        theAngelIslington
        // Etc.
    ]
    
    charge Boot oldKentRoad'' board'' |> printfn "Charged: %A"
    charge Battleship oldKentRoad'' board'' |> printfn "Charged: %A"
    
    
    let oldKentRoad''' = Street("Old Kent Road", 60, Brown, Empty, [|2;10;20;90;160;250|], Some Battleship)
    let whitechapelRoad''' = Street("Whitechapel Road", 60, Brown, Empty, [|2;10;20;90;160;250|], Some Battleship)

    let board''' = [
        go
        oldKentRoad'''
        communityChest
        whitechapelRoad'''
        incomeTax
        kingsCross
        theAngelIslington
        // Etc.
    ]
    
    charge Boot oldKentRoad''' board''' |> printfn "Charged: %A"
    charge Battleship oldKentRoad''' board''' |> printfn "Charged: %A"
    
    










