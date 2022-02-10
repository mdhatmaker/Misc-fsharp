
module Demo003


open Demo002
open System


let module_name = "Demo003"

type Result<'TSuccess, 'TFailure> =
    | Success of 'TSuccess
    | Failure of 'TFailure


/// Examples of using Result
let r1 = Success("hello")
let r2 = Failure(-1)

let IsSuccess x =
    match x with
    | Success _ -> true
    | _ -> false

IsSuccess r1
IsSuccess r2


let do1() =
    printfn "BEGIN MODULE %s\n" module_name

    // Initial World
    let key: Item = { Details =
                        { Name = "key1"
                          Description = "A cool-looking key" }}

    let allRooms = [

        { Id = RoomId "center"
          Details =
            { Name = "A central room"
              Description = "You are standing in a central room with exits in all directions. A single brazier lights the room." }
          Items = []
          Exits =
            { North = PassableExit ("You see a darkened passageway to the north.", RoomId "north1")
              South = PassableExit ("You see door to the south. A waft of cold air hits your face.", RoomId "south1")
              East = LockedExit ("You see a locked door to the east.", key, PassableExit ("You see an open door to the east.", RoomId "east1"))
              West = PassableExit ("You see an interesting room to the west.", RoomId "west1") }}

        { Id = RoomId "north1"
          Details =
            { Name = "A dark room"
              Description = "You are standing in a very dark room. You hear the faint sound of rats scurrying along the floor." }
          Items = []
          Exits =
            { North = NoExit None
              South = PassableExit ("You see a dimly lit room to the south.", RoomId "center")
              East = NoExit None
              West = NoExit None }}

        { Id = RoomId "south1"
          Details =
            { Name = "A cold room"
              Description = "You are standing in a room that feels very cold. Your breath instantly turns into a white puff."}
          Items = []
          Exits =
            { North = PassableExit ("You see an exit to the north. That room looks much warmer.", RoomId "center")
              South = NoExit None
              East = NoExit None
              West = NoExit None }}
            
    ]
    
    let player =
        { Details = { Name = "Luke"; Description = "Just your average adventurer." }
          Inventory = []
          Location = RoomId "center" }

    let gameWorld =
        { Rooms =
            allRooms
            |> Seq.map (fun room -> (room.Id, room))
            |> Map.ofSeq
          Player = player }


    //
    // ---------- Logic -----------
    //
    
    // Defining the Bind Function
    let bind processFunc lastResult =
        match lastResult with
        | Success s -> processFunc s
        | Failure f -> Failure f

    // Defining a Bind Operator (pipeline operator with bind attached)
    let (>>=) x f =
        bind f x

    // Defining the Switch Function
    let switch processFunc input =
        Success (processFunc input)

    let getRoom world roomId =
        match world.Rooms.TryFind roomId with
        | Some room -> Success room
        | None -> Failure "Room does not exist!"

    let describeDetails details =
        //printf "\n\n%s\n\n%s\n\n" details.Name details.Description
        sprintf "\n\n%s\n\n%s\n\n" details.Name details.Description

    (*// Match Expression
    let extractDetailsFromRoom (room: Room option) =
        match room with
        | Some(r) -> r.Details
        | None -> { Name = "No room!"; Description = "No room with that id found" }*)
    
    // update this function to reflect we will be getting a Result rather than just room
    let extractDetailsFromRoom (room: Room) =
        room.Details

    let describeCurrentRoom world =
        world.Player.Location
        |> getRoom world
        //|> (fun room -> extractDetailsFromRoom room |> describeDetails)
        |> (bind (switch extractDetailsFromRoom) >> bind (switch describeDetails))  // composition operator to merge two functions

    // Parameter Destructuring
    let north ({ North = northExit }: Exits) = northExit     // extract north exit from an Exit object
    let south ({ South = southExit }: Exits) = southExit
    let east ({ East = eastExit }: Exits) = eastExit
    let west ({ West = westExit }: Exits) = westExit

    let getCurrentRoom world =
        world.Player.Location
        |> getRoom world

    let setCurrentRoom world room =
        { world with
            Player = { world.Player with Location = room.Id }}

    let getExit direction exits =
        match (direction exits) with
        | PassableExit (_, roomId) -> Success roomId
        | LockedExit (_, _, _) -> Failure "There is a locked door in that direction."
        | NoExit (_) -> Failure "There is no room in that direction."

    // Railway-Oriented Programming (using bind function)

    let move direction world =
        world
        |> getCurrentRoom
        >>= (switch (fun room -> room.Exits))   // bind when it's going to take in result from previous operation
        >>= (getExit direction)
        >>= (getRoom world)
        >>= (switch (setCurrentRoom world))

    let displayResult result =
        match result with
        | Success s -> printf "%s" s
        | Failure f -> printf "%s" f

    gameWorld
    |> move south
    >>= (move north)            //|> bind (move north)
    //>>= (move west)             //|> bind (move west)
    >>= describeCurrentRoom     //|> bind (switch describeCurrentRoom)
    |> displayResult
    

    printfn "\nEND OF MODULE %s" module_name


    




