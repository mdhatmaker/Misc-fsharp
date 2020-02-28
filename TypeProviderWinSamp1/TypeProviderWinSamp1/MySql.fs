module MySql

open FSharp.Data.TypeProviders

(*
type dbSchema = SqlDataConnection<"Data Source=localhost;Initial Catalog=STUDENTS;Integrated Security=SSPI", ForceUpdate=true>

let PrintPurpleRow (row:dbSchema.ServiceTypes.Purple) =
    printfn "%i: %s" row.Id row.Fred
    printfn "%f" (row.Ryan.ToString())
    printfn "%i" (int row.Jeff)     // (System.Convert.ToInt16(row.Jeff))

let rec PrintPurple rows =
    match rows with
    | [] -> ()
    | head::tail ->
        PrintPurpleRow head
        printfn ""
        PrintPurple tail

*)


(*let sqlData = dbSchema.GetDataContext()

SqlData.Purple
|> Seq.toList
|> PrintPurple*)

printfn "--------------------------------------------------------------------------------"