namespace OSBuildings

open System.IO
open System.Diagnostics
open System.Drawing
open System.Runtime.InteropServices
open EGIS.ShapeFileLib


/// http://www.statsmapsnpix.com/2017/09/buildings-of-great-britain.html
/// https://www.streatmap.co.uk/map.srf?X=0.0&Y=0.0&z=5
/// https://www.dropbox.com/sh/kioja4ofr2azihn/AACSCu9nvAG_a-6wiM2y4TF8a?dl=0


//#r "Huitian.EGIS.ShapeFileLib.dll"
//open Huitian.EGIS.ShapeFileLib

module Perimeter =

    let shapeFilePath = @"D:\Users\mhatm\data\OS Buildings\wales_buildings_clipped.shp"
    let isMapShown = false  // set to true to display location(s) on web map

    /// Find largest building in UK
    /// (for our purposes: Largest building = Building with biggest parameter)
    let findLargestBuilding() =

        printfn "=============================================================="
        
        // for Windows, convert '&' to "^&" in URL (needed to start Process cmds)
        let validWindowsURL (url:string) = url.Replace("&", "^&")

        let openBrowser (url:string) =
            // https://brockallen.com/2016/09/24/process-start-for-urls-on-net-core/
            try
                Process.Start(url)
            with
                | :? System.Exception as ex ->
                    printfn "ERROR: %s" ex.Message

                    // hack because of this: https://github.com/dotnet/corefx/issues/10361
                    if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then
                        let winUrl = "/c start " + (validWindowsURL url)
                        Process.Start(new ProcessStartInfo("cmd", winUrl))
                    elif RuntimeInformation.IsOSPlatform(OSPlatform.Linux) then
                        Process.Start("xdg-open", url)
                    elif RuntimeInformation.IsOSPlatform(OSPlatform.OSX) then
                        Process.Start("open", url)
                    else
                        failwith "ERROR: Cannot open webpage in default browser"

        // display map of location (x=lat, y=lon) in default browser
        let showWebMap x y =
            printfn "   Showing web map for     LAT:%f    LON:%f ..." x y
            let url = sprintf "https://www.streetmap.co.uk/map.srf?X=%f&Y=%f&z=5" x y
            // Open UK Map in Browser (at lat/lon of biggest result)
            openBrowser url |> ignore

        let distance (p1:PointF, p2:PointF) =   // tuple for use in Seq.pairwise
            let dx = pown (p1.X - p2.X) 2
            let dy = pown(p1.Y - p2.Y) 2
            dx + dy |> sqrt

        let polyLength (ps: PointF seq) =
            ps
            |> Seq.pairwise
            |> Seq.sumBy distance

        let maxPerimeter filePath =
            printfn "--------------------------------------------------------------"
            printfn "File: %s" filePath
            use sf = new EGIS.ShapeFileLib.ShapeFile(filePath)
            printfn "Processing %s records..." (sf.RecordCount.ToString("#,##0"))
            Seq.init sf.RecordCount (sf.GetShapeData)
            |> Seq.maxBy (fun pointGroups ->
                pointGroups
                |> Seq.concat
                |> polyLength)
            |> Seq.head
        
        let displayResults (results: PointF[]) =
            //printfn "%d results: %A\n" (results.Length) results
            printfn "Result shape has  [ %d ]  points" (results.Length)
            results |> Array.toSeq |> polyLength |> printfn "Result shape perimeter: %.2f"
            printfn "--------------------------------------------------------------"
            // display location on web map (if isMapShown = true)
            if isMapShown then
                (showWebMap results.[0].X results.[0].Y)
            //else ()
            
        let maxPerimeterDir dirPath filePattern =
            Directory.EnumerateFiles(dirPath, filePattern)
            |> Seq.map (fun filename ->
                printfn "%s" filename
                let sf = new EGIS.ShapeFileLib.ShapeFile(filename)
                Seq.init sf.RecordCount (sf.GetShapeData))
            |> Seq.concat
            |> Seq.maxBy (fun pointGroups ->
                pointGroups
                |> Seq.concat
                |> polyLength)

        let maxPerimeterDir2 dirPath filePattern =
            Directory.EnumerateFiles(dirPath, filePattern)
            |> Seq.map (fun filename ->
                //printfn "%s" filename
                maxPerimeter filename
                |> displayResults )
            //|> printfn "%A"

        let stopWatch = System.Diagnostics.Stopwatch.StartNew()
        
        
        // Find largest building for ONE shape file
        let results1 = maxPerimeter shapeFilePath
        displayResults results1

        // Find largest building for ALL shape files in given directory
        let shapeDirectory = Path.GetDirectoryName(shapeFilePath)
        //maxPerimeterDir2 shapeDirectory "*.shp" |> printfn "%A"

        stopWatch.Stop()
        printfn "Elapsed time: %s ms" (stopWatch.Elapsed.TotalMilliseconds.ToString("#,##0.00"))

        printfn "=============================================================="
        printfn "Done.\n"






        (*************************** CODE GRAVEYARD ******************************)
        
        (*printfn "Processing %d records..." sf.RecordCount

        let maxpts =
            Seq.init sf.RecordCount (sf.GetShapeData)
            |> Seq.maxBy (fun pointGroups ->
                pointGroups
                |> Seq.concat
                |> polyLength)
        printfn "MAX: %A" maxpts
        printfn "Count[0]: %d  Count[1]:%d" (maxpts.[0].Length) (maxpts.[1].Length)
        printfn "%f\n%f" (polyLength maxpts.[0]) (polyLength maxpts.[1]) *)




        (*printf "Reading shape file..."
        let sf = new EGIS.ShapeFileLib.ShapeFile(shapePath)
        printfn "Done"
        printfn "--------------------------------------------------------------"
        *)

        (*for i in 0..9 do    //sf.RecordCount-1 do
                   printfn "==="
                   sf.GetShapeData(i)
                   |> Seq.iter(fun c -> 
                       printfn "---"
                       c
                           |> Seq.iter (fun p ->
                               printfn "%A" p)) *)


        (*Seq.init sf.RecordCount (fun i ->
            printfn "==="
            sf.GetShapeData(i)
            |> Seq.iter(fun c -> 
                printfn "---"
                c
                    |> Seq.iter (fun p ->
                        printfn "%A" p))) *)

        (*Seq.init sf.RecordCount (sf.GetShapeData)
        |> Seq.take 5
        |> Seq.iter (fun c ->
            printfn "---"
            c
            |> Seq.iter (fun p ->
                printfn "%A" p))
        printfn "--------------------------------------------------------------"
        *)

        (*let perims =
             results
             |> Seq.map polyLength
             |> Seq.iter (printfn "Perimeter: %f")
         printfn "--------------------------------------------------------------"
        *)