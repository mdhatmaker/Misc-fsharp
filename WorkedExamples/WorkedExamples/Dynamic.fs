module Dynamic


/// https://github.com/fsprojects/FSharp.Interop.Dynamic


open FSharp.Interop.Dynamic
open FSharp.Interop.Dynamic.Operators
open Dynamitey.DynamicObjects
open System.Reflection
open System.Linq
open Python.Runtime


/// target?Property, target?Property<-value, and target?Method(arg,arg2) allow you to
/// dynamically get/set properties and call methods
/// 
/// Also Dyn.implicitConvert,Dyn.explicitConvert, comparison operators and more.


let demo() =

    // MVC ViewBag
    //x.ViewBag?Name <- "George"

    // Dynamitey
    let ComBinder = LateType("System.Dynamic.ComBinder, System.Dynamic, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")

    let getPropertyNames (target:obj) =
        seq {
            yield! target.GetType().GetTypeInfo().GetProperties().Select(fun it -> it.Name)
            if (ComBinder.IsAvailable) then
                yield! ComBinder?GetDynamicDataMemberNames(target)
        }

    // Python Interop
    do
        use __ = Py.GIL()
        let math = Py.Import("math")
        math?cos(math?pi ?*? 2) |> printfn "%O"
        let sin = math?sin
        sin 5 |> printfn "%O"
        math?cos(5) ?+? sin(5) |> printfn "%O"
        math?pi |> printfn "%O"

    (*// SignalR (.net framework version)
    module private SignalRDemo =
        type MyHub =
            inherit Hub
            member x.Send (name : string) (message : string) =
                base.Clients.All?addMessage(name,message) |> ignore*)

    (*// System.Dynamic
    let ex1 = ExpandoObject()
    ex1?Test <- "Hi"            // set dynamic property
    printfn "%O" ex1?Test       // get dynamic*)








