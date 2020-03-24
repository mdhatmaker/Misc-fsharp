module ParsingCommandLine

/// https://fsharpforfunandprofit.com/posts/pattern-matching-command-line/



// set up a type to represent the options
type CommandLineOptions = {
    verbose: bool;
    subdirectories: bool;
    orderby: string;
    }


let demo() =

    // MYAPP [/V] [/S] [/O order]
    // /V    verbose
    // /S    include subdirectories
    // /O    order by. Parameter is one of
    //                 N - order by name.
    //                 S - order by size

    // Start by creating the internal model of the parameters

    // constants used later
    let OrderByName = "N"
    let OrderBySize = "S"






    printfn "------------------------------------------------------------------"