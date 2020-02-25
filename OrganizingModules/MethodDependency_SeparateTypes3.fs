/// Making types dumber
/// The Customer type still has some behavior embedded in it. A
/// more functional approach would be to pass a function only when
/// you need it. So let's remove the observer from the customer type,
/// and pass it as an extra parameter to the changeName function.
module MethodDependency_SeparateTypes =

    module DomainTypes =
        type Customer = {name:string}

    module Customer =
        open DomainTypes

        let changeName observer customer newName =
            let newCustomer = {customer with name=newName}
            observer newCustomer    // call the observer with the new customer
            newCustomer             // return the new customer

    module Observer =
        open DomainTypes

        let printNameChanged customer =
            printfn "Customer name changed to '%s' " customer.name

    module Test =
        open DomainTypes

        let observer = Observer.printNameChanged
        let customer = {name="Alice"}
        Customer.changeName observer customer "Bob"

    /// USE PARTIAL APPLICATION
    module TestWithPartialApplication =
        open DomainTypes

        let observer = Observer.printNameChanged

        // set up this partial appilcation only once (at the top of your module, say)
        let changeName = Customer.changeName observer

        // then call changeName without needing an observer
        let customer = {name="Alice"}
        changeName customer "Bob"

        


/// https://fsharpforfunandprofit.com/posts/removing-cyclic-dependencies/
