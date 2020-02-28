namespace Features

/// For more information on simulating the 'dynamic' type in F#:
/// https://github.com/fsprojects/FSharp.Interop.Dynamic

open FSharp.Interop.Dynamic


type DynamicModel() =
    member this.InvokeJavascriptFunction(fn, message) =
        fn message

    member this.Add(source, destination) =
        destination?computedResult <- source?value1 + source?value2

