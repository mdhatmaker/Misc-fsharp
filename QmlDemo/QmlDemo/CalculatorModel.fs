namespace Features

/// https://fsharpforfunandprofit.com/posts/classes/

open Qml.Net
open System


type CalculatorModel() =

    let mutable _isvalid = false            // private mutable values
    let mutable _computedResult = ""        //


    [<NotifySignal>]
    member this.ComputedResult              // changing a private mutable value
        with get() = _computedResult
        and set(value) = 
            if _computedResult = value
            then ()
            else
                _computedResult <- value
                this.ActivateSignal("computedResultChanged") |> ignore

    [<NotifySignal>]
    member this.IsValid                     // changing a private mutable value
        with get() = _isvalid
        and set(value) =
            if _isvalid = value
            then ()
            else
                _isvalid <- value
                this.ActivateSignal("isValidChanged") |> ignore

    member this.Add(inputValue1:string, inputValue2:string) =
        this.ComputedResult <- Convert.ToString(Decimal.Parse(inputValue1) + Decimal.Parse(inputValue2))
        
    member this.Subtract(inputValue1:string, inputValue2:string) =
        this.ComputedResult <- Convert.ToString(Decimal.Parse(inputValue1) - Decimal.Parse(inputValue2))

    member this.Multiply(inputValue1:string, inputValue2:string) =
        this.ComputedResult <- Convert.ToString(Decimal.Parse(inputValue1) * Decimal.Parse(inputValue2))
        
    member this.Divide(inputValue1:string, inputValue2:string) =
        let value1 = Decimal.Parse(inputValue1)
        let value2 = Decimal.Parse(inputValue2)

        if value2 = 0M
        then this.ComputedResult <- "Cannot divide by zero."
        else this.ComputedResult <- Convert.ToString(value1 / value2)





    

