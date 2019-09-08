#I "./packages"
#r "Google.DataTable.Net.Wrapper/lib/Google.DataTable.Net.Wrapper.dll"
#r "XPlot.GoogleCharts/lib/net45/XPlot.GoogleCharts.dll"

open XPlot.GoogleCharts

[ 1 .. 10 ] |> Chart.Line |> Chart.Show
