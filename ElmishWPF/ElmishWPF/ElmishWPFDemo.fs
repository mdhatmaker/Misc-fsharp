module ElmishWpfDemo

open System
open Elmish.WPF
open XamlViewsLib       // namespace from our C# Custom Controls library project


/// https://killalldefects.com/2019/12/30/getting-elmish-in-net-with-elmish-wpf/
/// https://github.com/elmish/Elmish.WPF/blob/master/TUTORIAL.md


type Model = {
    ClickCount: int
    Message: string
}


/// This is a discriminated union of the available messages from the user interface
type MessageType =
    | ButtonClicked
    | Reset



/// This is used to define the initial state of our applicaiton
let init() = {
    ClickCount = 0

    Message = "Hello Elmish.WPF"
}

/// The 'update' function takes in a 'MessageType' and the old 'Model' and outputs a
/// new 'Model'. One way to do that is using F#'s 'match' syntax as shown below.

/// This is the Reducer Elmish.WPF calls to generate a new model based on a message and an old model
let update (msg: MessageType) (model: Model) : Model =
    match msg with
    | ButtonClicked -> { model with ClickCount = model.ClickCount + 1 }
    | Reset -> init()

/// The 'binding' function configures an auto-generated data context object.
/// We are effectively just building a list of properties on an object and 
/// configuring how they behave.

/// Elmish uses this to provide the data context for your view based on a model
let bindings () : Binding<Model, MessageType> list = [
    // One-Way Bindings
    "ClickCount" |> Binding.oneWay (fun m -> m.ClickCount)
    "Message" |> Binding.oneWay (fun m -> m.Message)
    
    // Commands
    "ClickCommand" |> Binding.cmd ButtonClicked
    "ResetCommand" |> Binding.cmd Reset
]


let Run () =
    Program.mkSimpleWpf init update bindings
    |> Program.runWindow (MainView())


