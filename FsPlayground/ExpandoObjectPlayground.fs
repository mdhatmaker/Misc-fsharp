#I @"~/.nuget/packages"
//#r ""

open System
open System.Data
open System.Data.SqlClient

let (~~) (x:obj) =
  match x with
  | :? 't as t -> t
  | _ -> null

let rec mapper (reader : SqlDataReader) : list<'Value> =
  match reader.Read() with
  | false -> []
  | true ->
      let dc = new ExpandoObject()
      let dictionary = ~~dc : Dictionary<string, obj>
      [for i in [0 .. reader.FieldCount - 1] do
          dictionary.Add(reader.GetName(i), reader.GetValue(i))] @ mapper reader






