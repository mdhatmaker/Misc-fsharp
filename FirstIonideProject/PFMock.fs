module PFMock

open PF

// This is a simplified version of PrimeFactorsMock without the IsPrime override
type PrimeFactorsMock() =
    inherit PrimeFactors()

    member this.StepCount number =
        let mutable stepCount = 0L
        let facChecker (a, b) =
            stepCount <- stepCount + 1L
            PrimeFactors.getFactorType (a, b)
        this.getFactors number facChecker |> ignore
        stepCount



