open System
open MyApp

[<STAThread>]
[<EntryPoint>]
let main (_) =
  let application = App()
  application.Run () |> ignore
  0