namespace Features


open System
open Qml.Net


[<Signal("customSignal", NetVariantType.String)>]
type SignalsModel() =

    member this.RaiseSignal() =
        this.ActivateSignal("customSignal", String.Format("Signal was raised from .NET at {0}", DateTime.Now.ToLongTimeString()))

