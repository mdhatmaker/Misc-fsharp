module Episode_002

// https://youtu.be/hsTmLhnzRhE


let module_name = "Episode_002"

// Data Model Overview

// Record Type (name/value pairs)
type Details =
    {   Name: string
        Description: string }

type Item =
    {   Details: Details }

type RoomId =
    | RoomId of string

// Discriminated Union
type Exit =
    | PassableExit of string * destination: RoomId     // each entry can have data associated with it
    | LockedExit of string * key: Item * next: Exit    // next is next state of Exit
    | NoExit of string option                          // optional type

// Any option type can be Some or None
//let test = Some(42)
//let test = None

and Exits =
    {   North: Exit
        South: Exit
        East: Exit
        West: Exit }

and Room =
    {   Id: RoomId
        Details: Details
        Items: Item list
        Exits: Exits }

type Player =
    {   Details: Details
        Location: RoomId
        Inventory: Item list }

type World =
    {   Rooms: Map<RoomId, Room>
        Player: Player }



let do1() =
    printfn "BEGIN MODULE %s\n" module_name


    // Creating a Record Instance
    let firstRoom =
        {   Id = RoomId("RoomZero");
            Details = { Name = "First Room"; Description = "You're standing in a room"};
            Items = [];
            Exits =
                {   North = NoExit(None);
                    South = NoExit(None);
                    East = NoExit(None);
                    West = NoExit(None) }}

    printfn "%A" firstRoom

    printfn "\nEND OF MODULE %s" module_name


    




