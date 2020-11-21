module Library.Helpers.NugetHelper

//------------------------------------------------------------------------------
(*let packageFolder = "newtonsoft.json"
let version = "12.0.3"
let framework = "netstandard2.0"
let libName = "Newtonsoft.Json"*)
let inline packagePathname (framework:string) (libName:string) (version:string) =
    let packageFolder =  libName.ToLower()
    (sprintf @"%s/%s/lib/%s/%s.dll" packageFolder version framework libName)

let inline packagePathname' (libName:string) (version:string) = packagePathname "netstandard2.0" libName version

// Use these functions to print out the reference paths for required NuGet libs:
let printPackage libName version =
    printfn "\n#r \"%s\"" (packagePathname' libName version)

//------------------------------------------------------------------------------
let getFileList path =
    let filter = "*.*"  //"*.xml"
    System.IO.Directory.GetFiles(path, filter)
    |> Array.map System.IO.Path.GetFileName 

let getDirList path =
    System.IO.Directory.GetDirectories(path)
    |> Array.map System.IO.Path.GetFileName 

let inline availableVersions (libName:string) =
    let nugetRoot = "/Users/michael/.nuget/packages"
    let packageFolder =  libName.ToLower()
    let dir = (sprintf @"%s/%s" nugetRoot packageFolder)
    printfn "Getting versions for directory: '%s'" dir
    getDirList dir

//------------------------------------------------------------------------------

availableVersions "Newtonsoft.Json" |> Array.iter (printfn "%s") 
availableVersions "FSharp.Data" |> Array.iter (printfn "%s") 
availableVersions "Newtonsoft.Json.Schema" |> Array.iter (printfn "%s") 

//------------------------------------------------------------------------------

printPackage "Newtonsoft.Json" "12.0.3"
printPackage "Newtonsoft.Json.Schema" "3.0.13"

//------------------------------------------------------------------------------

#if INTERACTIVE
#I "/Users/michael/.nuget/packages"
#r "newtonsoft.json/12.0.3/lib/netstandard2.0/Newtonsoft.Json.dll"
#r "newtonsoft.json.schema/3.0.13/lib/netstandard2.0/Newtonsoft.Json.Schema.dll"
#endif


//==============================================================================

open System

//------------------------------------------------------------------------------

open Newtonsoft.Json

let getJsonNetJson obj =
    sprintf "I used to be %A but now I'm %s thanks to JSON.NET!" obj (JsonConvert.SerializeObject(obj))

//------------------------------------------------------------------------------

module SerializeDeserializeDemo =
    type Ty1 = {
        name : string
        age : int
    }

    let json1 = """{ "name":"Michael", "age":39 }"""
    let o1 = JsonConvert.DeserializeObject<Ty1>(json1)
    printfn "%s" (getJsonNetJson o1)

    let o2 = { name="Lori"; age=30 }
    printfn "%s" (getJsonNetJson o2)

//------------------------------------------------------------------------------

open Newtonsoft.Json.Schema
open Newtonsoft.Json.Linq

let schema = JSchema.Parse(@"{
      'type': 'object',
      'properties': {
        'name': {'type':'string'},
        'roles': {'type': 'array'}
      }
    }")

let user = JObject.Parse(@"{
      'name': 'Arnie Admin',
      'roles': ['Developer', 'Administrator']
    }")

let valid = user.IsValid(schema)

//------------------------------------------------------------------------------

open Newtonsoft.Json.Schema.Generation

type Account =
    {
        //[<EmailAddress>]
        [<JsonProperty("email", Required = Required.Always)>]
        Email : string
    }

let generator = JSchemaGenerator()
let schema = generator.Generate(typeof<Account>)
// {
//   "type": "object",
//   "properties": {
//     "email": { "type": "string", "format": "email" }
//   },
//   "required": [ "email" ]
// }

//------------------------------------------------------------------------------

open System.IO

let schema = JSchema.Parse(@"{
      'type': 'array',
      'item': {'type':'string'}
    }")

let reader = new JsonTextReader(new StringReader(@"[
      'Developer',
      'Administrator'
    ]"))

let validatingReader = new JSchemaValidatingReader(reader)
validatingReader.Schema <- schema

let serializer = JsonSerializer()
let roles = serializer.Deserialize<List<string>>(validatingReader)

//------------------------------------------------------------------------------












