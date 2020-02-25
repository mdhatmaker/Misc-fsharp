module MethodDependency_ParameterizedClasses_HOF =

    type Customer(name, observer) =

        let mutable name = name

        member this.Name
            with get() = name
            and set(value) =
                name <- value
                observer this


    // test
    let observer(c:Customer) =
        printfn "Customer name changed to '%s' " c.Name
    let customer = Customer("Alice", observer)
    customer.Name <- "Bob"




/// https://fsharpforfunandprofit.com/posts/removing-cyclic-dependencies/
