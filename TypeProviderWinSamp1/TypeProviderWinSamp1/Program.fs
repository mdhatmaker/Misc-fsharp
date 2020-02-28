module MainApp

/// https://fsharp.github.io/FSharp.Data/library/WorldBank.html


open Qml.Net
open Qml.Net.Runtimes

open Features



[<EntryPoint>]
let main argv =
    printfn "argv: %A" argv
    let sys_argv = argv
    //let sys_argv = Array.append argv [|"--style"; "material"|]


    //MyWorldBank.tryMe()

    

    RuntimeManager.DiscoverOrDownloadSuitableQtRuntime()
    //QQuickStyle.SetStyle("Material")
    use application = new QGuiApplication(sys_argv)
    use qmlEngine = new QQmlApplicationEngine()

    Qml.Net.Qml.RegisterType<CalculatorModel>("Features")
    
    qmlEngine.Load("Main.qml")

    application.Exec()


    (*System.Console.Write("Press any key... ")
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code*)

