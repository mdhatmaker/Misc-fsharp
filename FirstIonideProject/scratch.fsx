///
/// VECTOR 2D
/// 
type Vector2D (dx:double, dy:double) =
    let d2 = dx*dx+dy*dy

    member v.DX = dx
    member v.DY = dy
    member v.Length = sqrt d2
    member v.Scale(k) = Vector2D (dx*k, dy*k)


let v1 = Vector2D(2.5, 2.0)
let str (vector:Vector2D) =
    (sprintf "DX:%f  DY:%f" vector.DX vector.DY)
printfn "Vector: %s" (str v1)
printfn "Length: %f" (v1.Length)
printfn "Scaled by 2.0: %s" (str (v1.Scale 2.0))


///
/// TYPE PROVIDERS
///
#r "FSharp.Data.dll"
open FSharp.Data
open FSharp.Net

[<Literal>]
let sample = "http://api.openweathermap.org/data/2.5/weather?q=London"
let apiUrl = "http://api.openweathermap.org/data/2.5/weather?q="

type Weather = JsonProvider<sample>

let sf = Weather.Load(apiUrl + "San Francisco")
//sf.Sys.
printfn "%A" sf

#r "../TypeProviders/Debug/net40/Samples.WorldBank.dll"
let data = Sample.WorldBank.GetDataContext()
//data.Countries.

type TerraService = WsdlService<"http://msrmaps.com/TerraService2.asmx?WSDL">
let terraClient = TerraService.GetTerraSericeSoap()
    let myPlace = new TerraService.ServiceTypes.msrmaps.com.Place(City = "Redmond")
    let myLocation = terraClient.ConvertPlaceToLonLatPt(myPlace)
    printfn "Redmond Latitude: %f Longitude: %f" (myLocation.Lat) (myLocation.Lon)

