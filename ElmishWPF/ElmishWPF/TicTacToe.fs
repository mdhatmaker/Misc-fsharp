module TicTacToe

open System
open Elmish.WPF
open XamlViewsLib       // namespace from our C# Custom Controls library project


/// https://github.com/elmish/Elmish.WPF/blob/master/TUTORIAL.md


type Model = {
    WhoseTurn: int          // 1 or 2 to represent player for current turn
    Board: int array array  // 2-dimensional array (each cell holds 0, 1, or 2)
    ClickCount: int
    Message: string
}


/// This is a discriminated union of the available messages from the user interface
type MessageType =
    | GridClicked //of int array array
    | Reset



/// This is used to define the initial state of our applicaiton
let init() = {
    WhoseTurn = 0

    Board = [| [| 0; 0; 0 |]; [| 0; 0; 0 |]; [| 0; 0; 0 |] |]

    ClickCount = 0
    Message = "Here is my Model.Message field (string)"
}

/// The 'update' function takes in a 'MessageType' and the old 'Model' and outputs a
/// new 'Model'. One way to do that is using F#'s 'match' syntax as shown below.

/// This is the Reducer Elmish.WPF calls to generate a new model based on a message and an old model
let update (msg: MessageType) (model: Model) : Model =
    match msg with
    //| GridClicked board -> { model with Board = board }
    | GridClicked -> { model with WhoseTurn = (model.WhoseTurn+1) % 2 }
    | Reset -> init()

/// The 'binding' function configures an auto-generated data context object.
/// We are effectively just building a list of properties on an object and
/// configuring how they behave.

/// Elmish uses this to provide the data context for your view based on a model
let bindings () : Binding<Model, MessageType> list = [
    // One-Way Bindings
    "TurnPlayer" |> Binding.oneWay (fun m -> m.WhoseTurn + 1)
    "Message" |> Binding.oneWay (fun m -> m.Message)
    
    // Commands
    "ClickCommand" |> Binding.cmd GridClicked
    "ResetCommand" |> Binding.cmd Reset
]


let Run () =
    Program.mkSimpleWpf init update bindings
    |> Program.runWindow (TicTacToeView())


