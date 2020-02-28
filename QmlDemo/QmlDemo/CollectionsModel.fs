namespace Features


open System
open System.Collections.Generic


type Contact(name:string, phoneNumber:string) =
    let mutable _name = name
    let mutable _phoneNumber = phoneNumber

    member this.Name
        with get() = _name
        and set(value) = _name <- value
        
    member this.PhoneNumber
        with get() = _phoneNumber
        and set(value) = _phoneNumber <- value


type CollectionsModel() =

    let mutable _contacts = new List<Contact>()
    let con0 = new Contact(name="Paul Knopf", phoneNumber="525-525-52555")
    
    member this.AddContact(name:string, phoneNumber:string) =
        if String.IsNullOrEmpty(name) || String.IsNullOrEmpty(phoneNumber)
        then false
        else
            let con1 = new Contact(name=name, phoneNumber=phoneNumber)
            _contacts.Add( con1 )
            true

    member this.RemoveContact(index:int) =
        _contacts.RemoveAt(index)

    member this.Contacts
        with get() = _contacts

    




