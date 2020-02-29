namespace PhotoFrame.Logic


open PhotoFrame.Logic.BL
open PhotoFrame.Logic.Config
open PhotoFrame.Logic.UI.ViewModels
open PhotoFrame.Logic.UI.Views
open System
open System.ComponentModel
open System.IO
open System.Reflection
open System.Runtime.CompilerServices
open System.Linq




//https://gist.github.com/jbtule/8477768#file-nullcoalesce-fs
type NullCoalesce =  

    static member Coalesce(a: 'a option, b: 'a Lazy) = 
        match a with 
        | Some a -> a 
        | _ -> b.Value

    static member Coalesce(a: 'a Nullable, b: 'a Lazy) = 
        if a.HasValue then a.Value
        else b.Value

    static member Coalesce(a: 'a when 'a:null, b: 'a Lazy) = 
        match a with 
        | null -> b.Value 
        | _ -> a

Module Tools =
    
    let inline nullCoalesceHelper< ^t, ^a, ^b, ^c when (^t or ^a) : (static member Coalesce : ^a * ^b -> ^c)> a b = 
            // calling the statically inferred member
            ((^t or ^a) : (static member Coalesce : ^a * ^b -> ^c) (a, b))

    let inline (|??) a b = nullCoalesceHelper<NullCoalesce, _, _, _> a b



type AppModel() =

    

    member val UiDispatch:UiDispatchDelegate = null with get, set

    member val _instance:AppModel
    member this.Instance = _instance |?? (instance = new AppModel())


