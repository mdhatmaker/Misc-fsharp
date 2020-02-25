// This project is a "playground" for trying F# code

open System
open OSBuildings.Perimeter
open System.Threading.Tasks

[<EntryPoint>]
let main argv =

    OSBuildings.Perimeter.findLargestBuilding()

    do Task.Factory.StartNew(ParallelStuff.main1).Wait()

    

    0 // return an integer exit code
