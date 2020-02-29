// Quandl timeseries data and Charts

open System
open System.Collections.Generic

[<EntryPoint>]
let main argv =
    printfn "   *** Charts and Quandl in F# ***\n"

    /// Demo functions for various modules:
    //ChartModule.demo()
    //QuandlModule.Quandl.demo()


    /// Get Metadata for a TimeSeries Database
    let result = QuandlModule.Quandl.GetDatabaseMetadata "CHRIS"
    let rd = result.Database
    printfn "%A %A %A %A %A %A %A %A" rd.DatabaseCode rd.DatasetsCount rd.Description rd.Downloads rd.Id rd.Image rd.Name rd.Premium
    
    /// Get Timeseries Data and Metadata
    /// TODO: flesh this out

    /// Get Timeseries Metadata
    let result = QuandlModule.Quandl.GetTimeseriesMetadata ("CHRIS", "ICE_B1")
    let rd = result.Dataset
    printfn "COLUMNS....%A\n\n" rd.ColumnNames
    printfn "%A %A %A %A %A %A %A %A %A %A %A %A" rd.DatabaseCode rd.DatabaseId rd.DatasetCode rd.Description rd.Frequency rd.Id rd.Name rd.NewestAvailableDate rd.OldestAvailableDate rd.Premium rd.RefreshedAt rd.Type
    
    /// Get Filtered Timeseries Data
    let result = QuandlModule.Quandl.GetFilteredTimeseries ("CHRIS", "ICE_B1")
    let dataList:List<obj[]> = result.DatasetData.Data
    printfn "COLUMNS....%A\nROW COUNT....%i\n" result.DatasetData.ColumnNames dataList.Count
    

    /// Get Quandl data AND display XPlot chart:
    let displayChart label title dbTuple =
        let result = QuandlModule.Quandl.GetTimeseries dbTuple
        let dataList:List<obj[]> = result.DatasetData.Data
        printfn "COLUMNS....%A\nROW COUNT....%i\n" result.DatasetData.ColumnNames dataList.Count

        let chartInputs = ChartModule.quandlToChartInputs dataList
        let chartLabels = label
        let chart1 = ChartModule.createChart title [|"lines"|] [chartLabels] [chartInputs]
        ChartModule.showChart chart1

    displayChart "ICE_B1" "Brent Crude" ("CHRIS","ICE_B1")
    displayChart "ICE_G1" "GasOil" ("CHRIS","ICE_G1")



    0 // return an integer exit code
