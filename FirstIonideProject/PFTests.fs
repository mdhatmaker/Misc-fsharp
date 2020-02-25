namespace PrimeFactors

open PF
open PFMock
open NUnit.Framework
open FsUnit

[<TestFixture>]
module PFTests =

    [<TestCase(1L, [||])>]
    [<TestCase(2L, [|2L|])>]
    [<TestCase(3L, [|3L|])>]
    [<TestCase(4L, [|2L; 2L|])>]
    [<TestCase(6L, [|3L; 2L|])>]
    [<TestCase(8L, [|2L; 2L; 2L|])>]
    [<TestCase(16L, [|2L; 2L; 2L; 2L|])>]
    [<TestCase(773L, [|773L|])>]
    [<TestCase(8L, [|2L; 2L; 2L|])>]
    let prime_factors_of_number number expect =
        PrimeFactors().Of number |> should equal expect

    [<TestCase(1L, 0L)>]
    [<TestCase(4L, 1L)>]
    [<TestCase(773L, 26L)>]
    let step_count_of_number number stepCount =
        PrimeFactorsMock().StepCount number |> should equal stepCount



