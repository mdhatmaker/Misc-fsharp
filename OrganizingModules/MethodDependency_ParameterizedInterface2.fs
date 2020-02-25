module MethodDependency_ParameterizedInterface2 = 

    type ICustomerObserver<'T> = 
        abstract OnNameChanged : 'T -> unit
        abstract OnEmailChanged : 'T -> unit

    type Customer(name, email, observer:ICustomerObserver<Customer>) = 
        
        let mutable name = name
        let mutable email = email

        member this.Name 
            with get() = name
            and set(value) = 
                name <- value
                observer.OnNameChanged(this)

        member this.Email
            with get() = email
            and set(value) = 
                email <- value
                observer.OnEmailChanged(this)

    // test
    let observer2 = {
        new ICustomerObserver<Customer> with 
            member this.OnNameChanged c =     
                printfn "Customer name changed to '%s' " c.Name
            member this.OnEmailChanged c =     
                printfn "Customer email changed to '%s' " c.Email
        }
    let customer2 = Customer("Alice", "x@example.com",observer2)
    customer2.Name <- "Bob"
    customer2.Email <- "y@example.com"




https://fsharpforfunandprofit.com/posts/removing-cyclic-dependencies/
