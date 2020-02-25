/// ===========================================
/// Logging functions
/// ===========================================
module Logger = 

    open CommonLibrary 
    open DomainTypes

    let log twoTrackInput = 
        let success x = printfn "DEBUG. Success so far: %A" x; x
        let failure x = printfn "ERROR. %A" x; x
        doubleMap success failure twoTrackInput 