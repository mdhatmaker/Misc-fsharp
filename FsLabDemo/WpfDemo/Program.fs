open System
open WpfDemo

/// http://putridparrot.com/blog/creating-a-wpf-application-in-f/
/// ...says to select project properties and change the Application |
/// Output type from Console Application to Windows Application
/// ...says to add (to the Project) references to the following:
/// 1. PresentationCore
/// 2. PresentationFramework
/// 3. System.Xaml
/// 4. WindowsBase


[<STAThread>]
[<EntryPoint>]
let main (_) = 
    //let mainWindow = Application.LoadComponent(
    //                    new System.Uri("component/MainWindow.xaml", UriKind.Relative)) :?> Window
    //let application = new Application()
    //application.Run(mainWindow) |> ignore
    (*let application = MainWindowCode()
    application.Run () |> ignore*)
    /// https://alexatnet.com/quickstart-wpf-f-only-app-in-vscode/
    (*let appUri = Uri ("component/App.xaml", UriKind.Relative)
    let application = appUri    
                      |> Application.LoadComponent
                      :?> Application
    application.Run() |> ignore*)
    let application = App()
    application.Run() |> ignore
    0
