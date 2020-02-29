// 


open System
open System.Diagnostics
open System.Reflection
open System.Threading
open System.IO
open Qml.Net
//open PhotoFrame.Logic


[<EntryPoint>]
let main argv =
    printfn "A QML photo app in F#!"

    // Timer _checkTimer

    // QGuiApplication _app
    let mutable _app:QGuiApplication = null

    let CheckAndPrint =
        if not AppModel.HasInstance
        then ()
        else
            use proc = Process.GetCurrentProcess()
            GC.Collect(2, GCCollectionMode.Forced, true)
            let memBytes = proc.WorkingSet64
            let memKb = (float memBytes) / 1024.
            let memMb = (int memKb) / 1024
            let usedMbString = String.Format("{0:0.00} MB", memMb)
            printfn "Used memory: %s" usedMbString
            //_app?.Dispatch( fun ->
            _app.Dispatch( fun () ->
                AppModel.Instance.CurrentlyUsedMbString <- usedMbString
            )


    let _checkTimer = new Timer(fun e -> CheckAndPrint() )
    _checkTimer.Change(TimeSpan.FromSeconds(2.), TimeSpan.FromSeconds(5.))

    use app = new QGuiApplication(argv)

    _app <- app
    AppModel.UiDispatch += (fun a -> _app.Dispatch(a))
    QQmlApplicationEngine.ActivateMVVMBehavior()
    use engine = new QQmlApplicationEngine()

    Qml.Net.Qml.RegisterType<AppModel>("app", 1, 1)

    let assemblyDir = Path.GetDirectoryName((new Uri(Assembly.GetExecutingAssembly().Location)).LocalPath)
    let mainQmlPath = Path.Combine(assemblyDir, "UI", "QML", "main.qml")
    engine.Load(mainQmlPath)
    let result = app.Exec()
    _app <- null
    //result








    Console.Write("\nPress any key to exit ... ")
    Console.ReadKey() |> ignore
    //0 // return an integer exit code
    
    result