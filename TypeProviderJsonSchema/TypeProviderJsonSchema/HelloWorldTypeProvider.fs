namespace Samples.FSharp.HelloWorldTypeProvider

open System
open System.Reflection
//open ProviderImplementation.ProvidedTypes
open FSharp.Core.CompilerServices
open FSharp.Quotations
open FSharp.Data
//open FSharp.Data.TypeProviders
//open FSharp.TypeProviders


/// https://docs.microsoft.com/en-us/dotnet/fsharp/tutorials/type-providers/creating-a-type-provider



// This type defines the type provider. When compiled to a DLL, it can be added
// as a reference to an F# command-line compilation, script, or project.
[<TypeProvider>]
type SampleTypeProvider(config: TypeProviderConfig) as this =

    // Inheriting from this type provides implementations of ITypeProvider
    // in terms of the provided types below.
    inherit TypeProviderForNamespaces(config)
    
    let namespaceName = "Samples.HelloWorldTypeProvider"
    let thisAssembly = Assembly.GetExecutingAssymbly()

    // Make one provided type, called TypeN.
    //let makeOneProvidedtype (n:int) =


    // Now generate 100 types
    let types = [ for i in 1 .. 100 -> makeOneProvidedType i ]

    // And add them to the namespace
    do this.AddNamespace(namespaceName, types)


[<assembly:TypeProviderAssembly>]
do()

