module FableStuff



/// https://fable.io/

/// For FABLE, do the following:
/// install .NET Core SDK
/// install Node.js
/// type the following in terminal and open http://localhost:8080 in your browser after:
/// > git clone https://github.com/fable-compiler/fable2-samples
/// > cd fable2-samples/browser  
/// > npm install
/// > npm start


// Added these NuGet packages:
// Fable.Core
// Fable.JsonProvider
// Fable.SimpleHttp


/// With F# you can use built-in computation expressions and also extend them yourself
module ComputationExpressions1 =
    (*
    /// JS promieses made easy
    promise {
        let! res = Fetch.fetch url []
        let! txt = res.text()
        return txt.Length
    }
    *)

    // Declare your own computation expression
    type OptionBuilder() =
        member __.Bind(opt, binder) =
            match opt with Some value -> binder value | None -> None
        member __.Return(value) = Some value

    let option = OptionBuilder()

    let trySomething() = None
    let trySomethingElse() = None
    let andYetTrySomethingElse() = None

    option {
        let! x = trySomething()
        let! y = trySomethingElse()
        let! z = andYetTrySomethingElse()
        // Code will only hit this point if the three operations above return Some
        return x + y + z
    }



module UnitsOfMeasure =
    [<Measure>] type m
    [<Measure>] type s

    let distance = 12.0<m>
    let time = 6.0<s>

    //let thisWillFail = distance + time  // ERROR: unit of measure 'm' does not match uofm 's'

    let thisWorks = distance / time    // 2.0<m/s>



(*
module TypeProviders =
    let [<Literal>] JSON_URL = "https://jsonplaceholder.typeicode.com/todos"

    // Type is created automatically from the url
    type Todos = Fable.JsonProvider.Generator<JSON_URL>

    async {
        let! (_, res) = Fable.SimpleHttp.get url
        let todos = Todos.ParseArray res
        for todo in todos do
            // If the JSON schema changes, this will fail compilation
            printfn "ID %i, USER: %i, TITLE %s, COMPLETED %b"
                todo.id
                todo.userId
                todo.title
                todo.completed
    }

*)


