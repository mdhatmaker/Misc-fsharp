module MethodDependencyExample =

    type Customer(name, observer:CustomerObserver) =
        let mutable name = name
        member this.Name
            with get() = name
            and set(value) =
                name <- value
                observer.OnNameChanged(this)

    and CustomerObserver() =
        member this.OnNameChanged(c:Customer) =
            printfn "Customer name changed to '%s' " c.Name

    // test
    let observer = new CustomerObserver()
    let customer = Customer("Alice", observer)
    customer.Name <- "Bob"




/// https://fsharpforfunandprofit.com/posts/removing-cyclic-dependencies/
