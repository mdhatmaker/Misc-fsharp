
module LearnClasses


// Class
type Product (code:string, price:float) =
  let isFree = price=0.0
  new (code) = Product(code, 0.0)
  member this.Code = code
  member this.IsFree = isFree



// Interface
/// https://fsharpforfunandprofit.com/posts/interfaces/
type IPrintable =
  abstract member Print : unit -> unit
// create a class that implements the IPrintable interface
type PrintableClass () =
  interface IPrintable with
    member this.Print () = printfn "Howdy!"

// our demoPrint function takes an IPrintable as its only argument
let execPrintMethodOn (printInstance:IPrintable) = printInstance.Print ()

let instance = new PrintableClass ()  // create instance of PrintableClass
execPrintMethodOn instance            // pass this instance to our execPrintMethodOn function

let interfaceInstance = instance :> IPrintable   // cast to the IPrintable interface


// Struct
type ProductValue =
  struct
    val code:string
    val price:float
    new(code) = { code = code; price = 0.0 }
  end



let p1 = Product("X123", 9.99)
let p2 = Product("X123")

let pr1 = ProductValue()
let pr2 = ProductValue("X123")


