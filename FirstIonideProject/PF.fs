module PF


/// https://codereview.stackexchange.com/questions/189062/mocking-an-f-primality-tester



// Instead of an active pattern, we use this discriminated union type.
type FactorType =
    | Factors of uint64 * uint64
    | NonFactor

// Function prototype for functions testing if a number is a factor of
// another (changed domain type from int64 to uint64).
type FactorChecker = uint64 * uint64 -> FactorType

type PrimeFactors() =
        
    // This function is made static because it has nothing to do with the
    // class itself and because we want to reuse it in the derived Mock class
    // (checks if b is a factor of a).
    static member getFactorType (a, b) =
        match a % b with
        | r when r = 0UL -> Factors(b, a/b)
        | _ -> NonFactor

    // This could be static as well, but we made it a member. It would be
    // nice to declare it protected, but that's not an option in F#.
    // In order to let the client provide the function to check for factors,
    // the last argument is a function of type FactorChecker.
    member this.getFactors number (factorChecker: FactorChecker) =
        if number < 2UL then
            []
        else
            // The stop condition is the (uint64 sqrt) of n
            let max n = n |> float |> sqrt |> uint64

            let rec getPrimes num i acc =
                match i with
                | _ when i > max num -> num::acc
                | _ ->
                    match factorChecker (num, i) with
                    | Factors (a,b) -> getPrimes b a (a::acc)
                    | NonFactor -> getPrimes num (i + 1UL) acc

            getPrimes number 2UL []

    member this.Of (number: uint64) : uint64 list = this.getFactors number PrimeFactors.getFactorType
        












