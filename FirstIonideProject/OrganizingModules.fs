namespace OrganizingModules



/// https://fsharpforfunandprofit.com/posts/recipe-part3/


///          LAYER DIAGRAM
/// =====================================================================
/// Presentation Layer          ORANGE           Presentation functions
///                                              Presentation types
/// Persistence                 green            Persistence impl
///                                              Persistence interfaces
/// Domain Layer                BLUE             Domain functions
///                                              Domain types
/// Infrastructure              purple           Infrastructure
///



///       PROJECT ORGANIZATION
/// ====================================
/// InfrastructureTypes.fs      purple
/// DomainTypes.fs              BLUE
/// Logger.fs                   purple
/// SMTP.fs                     purple
/// Validation.fs               BLUE
/// SendEmail.fs                BLUE
/// DbTypes.fs                  green
/// CustomerRepository.fs       green
/// UITypes.fs                  ORANGE
/// UserInterface.fs            ORANGE
/// UseCases.fs                 ORANGE
/// Main.fs                     ----



///
/// APPROACH #1: types are definied inside the module along with their related functions
/// If there is only one primary type, it is often given a simple name such as "T" or the
/// name of the module.
///
module Person =

    type T = {First:string; Last:string}

    // constructor
    let create first last =
        {First=first; Last=last}

    // method that works on the type
    let fullName {First=first; Last=last} =
        first + " " + last

///
/// APPROACH #2: types are declared in the same file, but outside any module
///
type PersonType = {First:string; Last:string}

module PersonB =
    let create first last =                     // constructor
        {First=first; Last=last}
    let fullName {First=first; Last=last} =     // method that works on the type
        first + " " + last


///
/// APPROACH #3: the type is declared in a special "types-only" module (typically in
/// a different file).
//
// ===========================
// File: DomainTypes.fs
// ===========================
// namespace Example
//
// // "types-only" module
// [<AutoOpen>]
// module DomainTypes =
//     type Person = {First:string; Last:string}
//     type OtherDomainType = ...
//     type ThirdDomainType = ...
//
// ===========================
// File: Person.fs
// ===========================
// namespace Example
//
// // declare a module for functions that work on the type
// module Person =
//     let create first last =                  // constructor
//         {First=first; Last=last}
//     let fullName {First=first; Last=last} =  // method that works on the type
//         first + " " + last
//
//
// In this example, both the type and the module are called Person. This is not normally
// a problem in practice, as the compiler can normally figure out what you want.
// If you write this:
//     let f (p:Person) = p.First
// The compiler will understand that you are referring to the Person type.
// On the other hand, if you write this:
//     let g () = Person.create "Alice" "Smith"
// Then the compiler will understand that you are referring to the Person module.

module TryIt =
    open Person

    let tryPerson() =
        let p1 = Person.create "David" "Sanders"
        printfn "%s" (Person.fullName p1)
        // type itself is 'Person.T'



///
/// Module Guidelines
///
/// If a type is shared among multiple modules, then put it in a special types-only module.
///
/// -if a type is used globally, then put it in a module "DomainTypes" or "DomainModel"
///  (put it early in the compilation order)
///
/// -if a type is used only in a subsystem, such as UI modules, then put it in a module "UITypes"
///  (put it just before the other UI modules in the compilation order)
///
/// See also: Cycles and modularity in the wild 
///           https://fsharpforfunandprofit.com/posts/cycles-and-modularity-in-the-wild/
///      and: Cyclic dependencies are evil
///           https://fsharpforfunandprofit.com/posts/cyclic-dependencies/
///      and: Refactoring to remove cyclic dependencies
///           https://fsharpforfunandprofit.com/posts/removing-cyclic-dependencies/




/// Back in the "Organizing modules in a project" article (https://fsharpforfunandprofit.com/posts/recipe-part3/)
/// we have code organized into the following modules (each one should be its own file):
///
/// CommonLibrary.fs        Common types and functions shared across multiple projects
/// DomainTypes.fs          Global types for this project
/// Logger.fs               Logging functions
/// Validation.fs           Validation functions
/// CustomerRepository.fs   Database functions
/// UseCases.fs             All the use cases or services in one place



///
/// Refactoring to remove cyclic dependencies
///
/// https://fsharpforfunandprofit.com/posts/removing-cyclic-dependencies/

/////////////////////////////////////////////////////////////////////////////////////////
/// Dealing with a "method dependency"
/////////////////////////////////////////////////////////////////////////////////////////

/// Using the "and" keyword
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
    let customer = Customer("Alice",observer)
    customer.Name <- "Bob"

/// But 'and' has a number of problems, and using it is generally discouraged except as
/// a last resort.
///
/// First, it only works for types declared in the same module. You can’t use it across
/// module boundaries.
///
/// Second, it should really only be used for tiny types. If you have 500 lines of code
/// between the 'type' and the 'and', then you are doing something very wrong.


/// Introducing parameterization
/// If we think about the example code, do we really need a special 'CustomerObserver' class?
/// Why have we restricted it to 'Customer' only? Can’t we have a more generic observer class?
///
/// So why don’t we create a 'INameObserver<'T>' interface instead, with the same 'OnNameChanged'
/// method, but the method (and interface) parameterized to accept any class?
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

    // test2
    // uses "object expressions" which allow you to instantiate an interface directly:
    let observer2 = {
        new INameObserver<Customer> with 
            member this.OnNameChanged c =     
                printfn "Customer name changed to '%s' " c.Name
        }
    let customer2 = Customer("Alice", observer2)
    customer2.Name <- "Bob"

/// Using "object expressions" we can instantiate an interface directly. In the following code,
/// we have eliminated the 'CustomerObserver' class and created the 'INameObserver' directly:
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

/// Using functions instead of parameterization
/// Why not just pass in a simple function that is called when the name changes?
/// This only works when the interface being replaced is simple, but even so, this
/// approach can be used more often than you might think.
module MethodDependency_ParameterizedClasses_HOF  = 

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


/// A more functional approach: separating types from functions
/// In this more "functional design" we can separate the types themselves from the functions
/// that act on those types.
module MethodDependencyExample_SeparateTypes = 

    module DomainTypes = 
        type Customer = { name:string; observer:NameChangedObserver }
        and  NameChangedObserver = Customer -> unit


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

    // test
    module Test = 
        open DomainTypes

        let observer = Observer.printNameChanged 
        let customer = {name="Alice"; observer=observer}
        Customer.changeName customer "Bob"

/// Just like above, we can remove the mutual dependency between 'Customer' and 'CustomerObserver'
/// by eliminating the observer type and embedding a function directly in the 'Customer' data structure.
module MethodDependency_SeparateTypes2 = 

    module DomainTypes = 
        type Customer = { name:string; observer:Customer -> unit}

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

/// Making types dumber
/// The 'Customer' type still has some behavior embedded in it. In many cases, there is no need
/// for this. A more functional approach would be to pass a function only when you need it.
///
/// So let's remove the 'observer' from the customer type, and pass it as an extra parameter
/// to the 'changeName' function.
module MethodDependency_SeparateTypes3 = 

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

    /// Remember, we can use partial application to set up a function with the observer
    /// "baked in" and then use that function everywhere, without needing to pass in an
    /// observer every time you use it.
    module TestWithPartialApplication = 
        open DomainTypes

        let observer = Observer.printNameChanged 

        // set up this partial application only once (at the top of your module, say)
        let changeName = Customer.changeName observer 

        // then call changeName without needing an observer
        let customer = {name="Alice"}
        changeName customer "Bob"

/// Generic logic
/// We can rewrite the 'changeName' function as a completely generic library function. Our
/// new function will allow any observer function to "hook into" the result of any other function.
    let hook2 observer f param1 param2 =
        let y = f param1 param1     // do something to make a result value
        observer y                  // call the observer with the result value
        y                           // return the result value

/// We named the function 'hook2' because the function 'f' being "hooked into" has two
/// parameters. Here's another version for functions that have one parameter:
    let hook observer f param1 =
        let y = f param1            // do something to make a result value
        observer y                  // call the observer with the result value
        y                           // return the result value

/// Again, we create a partially applied 'changeName' function, but this time we create it by
/// passing the observer and the hooked function to 'hook2':
///    let observer = Observer.printNameChanged
///    let changeName = hook2 observer Customer.changeName

/// Here's the refactored code that utilizes the 'hook2' function to "wire up" an observer:
module MethodDependency_SeparateTypes_WithHookFunction = 

    [<AutoOpen>]
    module MyFunctionLibrary = 

        let hook observer f param1 = 
            let y = f param1 // do something to make a result value 
            observer y       // call the observer with the result value
            y                // return the result value

        let hook2 observer f param1 param2 = 
            let y = f param1 param2 // do something to make a result value
            observer y              // call the observer with the result value
            y                       // return the result value

    module DomainTypes = 
        type Customer = { name:string}

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

        // then call changeName without needing an observer
        let customer = {name="Alice"}
        changeName customer "Bob"



/////////////////////////////////////////////////////////////////////////////////////////
/// Dealing with a "structural dependency"
/////////////////////////////////////////////////////////////////////////////////////////

/// With a "structural dependency", each type stores a value of the other type:
/// -Type A stores a value of type B in a property
/// -Type B stores a value of type A in a property

/// Consider an 'Employee' who works at a 'Location'...

module StructuralDependencyExample = 

    type Employee(name, location:Location) = 
        member this.Name = name
        member this.Location = location

    and Location(name, employees: Employee list) = 
        member this.Name = name
        member this.Employees  = employees






/////////////////////////////////////////////////////////////////////////////////////////
/// Dealing with a "inheritance dependency"
/////////////////////////////////////////////////////////////////////////////////////////

/// -Type A stores a value of type B in a property
/// -Type B inherits from type A

/// Example: UI control hierarchy, where every control belongs to a top-level "Form", and
/// the Form itself is a Control.
module InheritanceDependencyExample = 

    type Control(name, form:Form) = 
        member this.Name = name

        abstract Form : Form
        default this.Form = form

    and Form(name) as self = 
        inherit Control(name, self)

    // test
    let form = new Form("form")       // NullReferenceException!
    let button = new Control("button",form)

/// A better design, which also fixes the constructor error, is to make Control an abstract
/// class instead, and distinguish between non-form child classes (which do take a form in their
/// constructor) and the Form class itself, which doesn't.
module InheritanceDependencyExample2 = 

    [<AbstractClass>]
    type Control(name) = 
        member this.Name = name

        abstract Form : Form

    and Form(name) = 
        inherit Control(name)

        override this.Form = this

    and Button(name,form) = 
        inherit Control(name)

        override this.Form = form

    // test
    let form = new Form("form")       
    let button = new Button("button",form)

/// To remove the circular dependency, we can parameterize the classes in the usual way:
module InheritanceDependencyExample_ParameterizedClasses = 

    [<AbstractClass>]
    type Control<'Form>(name) = 
        member this.Name = name

        abstract Form : 'Form

    type Form(name) = 
        inherit Control<Form>(name)

        override this.Form = this

    type Button(name,form) = 
        inherit Control<Form>(name)

        override this.Form = form


    // test
    let form = new Form("form")       
    let button = new Button("button",form)

/// TODO: Create a functional design that remedies this "structural dependency".
/// For truly functional design, we probably would not be using inheritance at all. Instead,
/// we would use composition in conjuncion with parameterization.

































