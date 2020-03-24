module ClassDemo

/// Classes, Inheritance, and Interfaces

// TODO: classes and interfaces web pages below...

/// https://fsharpforfunandprofit.com/posts/classes/
/// https://fsharpforfunandprofit.com/posts/inheritance/
/// https://fsharpforfunandprofit.com/posts/interfaces/


/// DEFINING AN INTERFACE
type MyInterface =          // <- no parens! (very subtle difference)
   // abstract method
   abstract member Add: int -> int -> int

   // abstract immutable property
   abstract member Pi : float 

   // abstract read/write property
   abstract member Area : float with get,set

/// DEFINING AN ABSTRACT BASE CLASS
[<AbstractClass>]
type AbstractBaseClass() =
   // abstract method
   abstract member Add: int -> int -> int

   // abstract immutable property
   abstract member Pi : float 

   // abstract read/write property
   abstract member Area : float with get,set




/// CLASS THAT INHERITS FROM ANOTHER CLASS:
/// type DerivedClass(param1, param2) =
///    inherit BaseClass(param1)    // 'inherit' signals that DerivedClass inherits from BaseClass
type BaseClass(param1) =
   member this.Param1 = param1

type DerivedClass(param1, param2) =
   inherit BaseClass(param1)    // contains both the class to inherit from *and* its constructor
   member this.Param2 = param2

// test
let derived = new DerivedClass(1,2)
printfn "param1=%O" derived.Param1
printfn "param2=%O" derived.Param2


/// ABSTRACT AND VIRTUAL METHODS
// concrete function definition
let Add x y = x + y

// function signature
// val Add : int -> int -> int

/// So to define an abstract method, we use the signature syntax, along with the
/// 'abstract member' keywords:
[<AbstractClass>]
type BaseClassAb() =
   abstract member Add: int -> int -> int   // Notice equals sign is replaced with a colon

/// Abstract immutable property is defined similarly to abstract method:
[<AbstractClass>]
type BaseClass'() =
   abstract member Pi : float

/// If the abstract property is read/write, you add the get/set keywords
[<AbstractClass>]
type BaseClass''() =
    abstract Area : float with get, set


/// DEFAULT IMPLEMENTATIONS (BUT NO VIRTUAL METHODS)
/// To provide a default implementation of an abstract method in the
/// base class, use the 'default' keyaord instead of the 'member' keyword
// with default implementations
type BaseClassD() =
   // abstract method
   abstract member Add: int -> int -> int
   // abstract property
   abstract member Pi : float 

   // defaults
   default this.Add x y = x + y
   default this.Pi = 3.14


/// ABSTRACT CLASSES
/// If at least one abstract method does *not* have a default implementation,
/// then the entire class is abstract:
[<AbstractClass>]
type AbstractBaseClass'() =
   // abstract method
   abstract member Add: int -> int -> int

   // abstract immutable property
   abstract member Pi : float 

   // abstract read/write property
   abstract member Area : float with get,set


/// OVERRIDING METHODS IN SUBCLASSES
/// To override an abstract method or property in a subclass, use the
/// 'override' keyword instead of the 'member' keyword:
[<AbstractClass>]
type Animal() =
   abstract member MakeNoise: unit -> unit 

type Dog() =
   inherit Animal() 
   override this.MakeNoise () = printfn "woof"

// test
// let animal = new Animal() // error creating ABC
let dog = new Dog()
dog.MakeNoise()


/// And to call a base method, use the 'base' keyword, just as in C#:
type Vehicle() =
   abstract member TopSpeed: unit -> int
   default this.TopSpeed() = 60

type Rocket() =
   inherit Vehicle() 
   override this.TopSpeed() = base.TopSpeed() * 10

// test
let vehicle = new Vehicle()
printfn "vehicle.TopSpeed = %i" <| vehicle.TopSpeed()
let rocket = new Rocket()
printfn "rocket.TopSpeed = %i" <| rocket.TopSpeed()

















