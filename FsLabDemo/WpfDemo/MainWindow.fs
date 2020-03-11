namespace WpfDemo  

/// https://www.c-sharpcorner.com/article/create-wpf-application-with-f-sharp-and-fsxaml/

open System
open FsXaml  


type MainWindow = XAML<"component/MainWindow.xaml">

(*type MainWindowCode() =
    inherit MainWindow()    // contains both the class to inherit from *and* its constructor 
*)
type MainViewController() =
    //inherit UserControlViewController<MainWindow>()
    
    //new() = { inherit XAMLFileBrowserControl(); }
    let mutable FileNameChanged = new Event<EventHandler, EventArgs>()

    (*member this.SelectedText1 =
        if this.Selected1 >= 0
        then Some (this.ChartDemoList.Items.[this.Selected1].ToString())
        else None

    member this.Selected1
        with get() = this.ChartDemoList.SelectedIndex
        and set(value) = this.ChartDemoList.SelectedIndex <- value

        
    override this.LaunchDemoButton_Click(sender:obj, e:System.Windows.RoutedEventArgs) =
        (*let openFileDlg = new Microsoft.Win32.OpenFileDialog()
        if openFileDlg.ShowDialog().HasValue
        then this.FileName <- openFileDlg.FileName*)
        match this.SelectedText1 with
        | Some txt -> printfn "LaunchDemo: %s" txt
        | _ -> printfn "LaunchDemo: No list item selected"*)

    
    (*override this.FBCTextBox_TextChanged(sender:obj, e:System.Windows.Controls.TextChangedEventArgs) =
        e.Handled <- true
        (*if (FileNameChanged <> null)
        then FileNameChanged.Trigger(this, EventArgs.Empty)*)
        FileNameChanged.Trigger(this, EventArgs.Empty)*)