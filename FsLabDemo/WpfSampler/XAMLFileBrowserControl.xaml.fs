module XAMLFileBrowserControl


/// https://www.c-sharpcorner.com/UploadFile/mahesh/user-control-in-wpf/

open FsXaml
open System

//open McXamlLib


type XAMLFileBrowserControl = XAML<"XAMLFileBrowserControl.xaml">

type XAMLFileBrowser() =
    inherit XAMLFileBrowserControl()    // contains both the class to inherit from *and* its constructor 
    
    //new() = { inherit XAMLFileBrowserControl(); }
    let mutable FileNameChanged = new Event<EventHandler, EventArgs>()

    member this.FileName
        with get() = this.FBCTextBox.Text
        and set(value) = this.FBCTextBox.Text <- value

        
    override this.FBCButton_Click(sender:obj, e:System.Windows.RoutedEventArgs) =
        let openFileDlg = new Microsoft.Win32.OpenFileDialog()
        if openFileDlg.ShowDialog().HasValue
        then this.FileName <- openFileDlg.FileName

    
    override this.FBCTextBox_TextChanged(sender:obj, e:System.Windows.Controls.TextChangedEventArgs) =
        e.Handled <- true
        (*if (FileNameChanged <> null)
        then FileNameChanged.Trigger(this, EventArgs.Empty)*)
        FileNameChanged.Trigger(this, EventArgs.Empty)