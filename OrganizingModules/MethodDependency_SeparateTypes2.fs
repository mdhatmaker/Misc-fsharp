module MethodDependency_SeparateTypes2 =

    module DomainTypes =
        type Customer = { name:string; observer:Customer -> unit }

    module Customer =
        open DomainTypes

        let changeName customer newName =
            let newCustomer = {customer with name=newName}
            customer.observer newCustomer
            newCustomer     // return the new customer

    module Observer =
        open DomainTypes

        let printNameChanged customer =
            printfn "Customer name changed to '%s' " customer.name

    module Test =
        open DomainTypes

        let observer = Observer.printNameChanged
        let customer = {name="Alice"; observer=observer}
        Customer.changeName customer "Bob"





/// https://fsharpforfunandprofit.com/posts/removing-cyclic-dependencies/
