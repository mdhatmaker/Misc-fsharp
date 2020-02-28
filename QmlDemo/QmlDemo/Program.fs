
/// For more info about F# classes:
/// https://fsharpforfunandprofit.com/posts/classes/

open Qml.Net
open Qml.Net.Runtimes

open Features

[<EntryPoint>]
let main argv =
    printfn "Let's play with some QML/Qt5 in F#!"

    
    RuntimeManager.DiscoverOrDownloadSuitableQtRuntime()
    
    QQuickStyle.SetStyle("Material")
    
    use application = new QGuiApplication(argv)
    use qmlEngine = new QQmlApplicationEngine()
    
    Qml.Net.Qml.RegisterType<SignalsModel>("Features")
    Qml.Net.Qml.RegisterType<NotifySignalsModel>("Features")
    Qml.Net.Qml.RegisterType<AsyncAwaitModel>("Features")
    Qml.Net.Qml.RegisterType<NetObjectsModel>("Features")
    Qml.Net.Qml.RegisterType<DynamicModel>("Features")
    Qml.Net.Qml.RegisterType<CalculatorModel>("Features")
    Qml.Net.Qml.RegisterType<CollectionsModel>("Features")
    
    qmlEngine.Load("Main.qml")

    application.Exec()


    System.Console.Write("Press any key... ")
    System.Console.ReadKey() |> ignore
    0 // return an integer exit code

    
