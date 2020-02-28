namespace Features


open System
open Qml.Net


type NotifySignalsModel() =
    let mutable _bindableProperty = ""

    [<NotifySignal>]
    member this.BindableProperty
        with get() = _bindableProperty
        and set(value) = 
            if _bindableProperty = value
            then ()
            else
                _bindableProperty <- value
                this.ActivateSignal("bindablePropertyChanged") |> ignore

    member this.ChangeBindableProperty() =
        this.BindableProperty <- DateTime.Now.ToLongTimeString()



