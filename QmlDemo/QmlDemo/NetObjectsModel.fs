namespace Features


type NetObject() =
    let mutable _property = ""
    
    member this.Property
        with get() = _property
        and set(value) = _property <- value

    member this.UpdateProperty(value:string) =
        this.Property <- value


type NetObjectsModel() =

    member this.GetNetObject() =
        new NetObject()
 
    

        