/// We rewrite the Customer changeName function as a completely
/// generic library function. Our new function will allow any
/// observer function to "hook into" the result of any other function.
module MethodDependency_SeparateTypes_WithHookFunction =

    [<AutoOpen>]
    module MyFunctionLibrary =

        let hook observer f param1 =
            let y = f param1    // do something to make a result value
            observer y          // call the observer with the result value
            y                   // return the result value

        let hook2 observer f param1 param2 =
            let y = f param1 param2 // do something to make a result value
            observer y              // call the observer with the result value
            y                       // return the result value

    module DomainTypes =
        type Customer = {name:string}

    module Customer =
        open DomainTypes

        let changeName customer newName =
            {customer with name=newName}

    module Observer =
        open DomainTypes

        let printNameChanged customer =
            printfn "Customer name changed to '%s' " customer.name

    module TestWithPartialApplication =
        open DomainTypes

        // set up this partial application only once (at the top of your module, say)
        let observer = Observer.printNameChanged
        let changeName = hook2 observer Customer.changeName

        // then call changeName wihtout needing an observer
        let customer = {name="Alice"}
        changeName customer "Bob"
