module MethodDependency_ParameterizedInterface =

    type INameObserver<'T> =
        abstract OnNameChanged : 'T -> unit

    type Customer(name, observer:INameObserver<Customer>) =
        let mutable name = name
        member this.Name
            with get() = name
            and set(value) =
                name <- value
                observer.OnNameChanged(this)

    type CustomerObserver() =
        interface INameObserver<Customer> with
            member this.OnNameChanged c =
                printfn "Customer name changed to '%s' " c.Name

    // test
    let observer = new CustomerObserver()
    let customer = Customer("Alice", observer)
    customer.Name <- "Bob"

    // use "object expression" to instantiate an interface directly
    let observer2 = {
        new INameObserver<Customer> with
            member this.OnNameChanged c =
                printfn "Customer name changed to '%s' " c.Name
        }
    let customer2 = Customer("Alice", observer2)
    customer2.Name <- "Bob"
    



/// https://fsharpforfunandprofit.com/posts/removing-cyclic-dependencies/
