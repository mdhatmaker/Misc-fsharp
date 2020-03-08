module WpfTutorial

/// WPF Demo
/// https://jyliao.blogspot.com/2007/11/learning-wpf-with-f-working-with.html

//#light 
(*#I @"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0"
#r @"WindowsBase.dll"
#r @"PresentationCore.dll"
#r @"PresentationFramework.dll"*)

open Microsoft.Win32
open System
open System.IO
open System.Windows
open System.Windows.Controls
open System.Windows.Documents
open System.Windows.Input
open System.Windows.Media
open System.Windows.Media.Imaging
open System.Windows.Controls.Primitives
open System.ComponentModel
//open System.Windows.Forms



(* From Chap 4 - ClickTheButton.cs *)
type ClickTheButton = class
   inherit Window //as base  
   
   new () as this = {} then
      this.Title <- "Click the Button"
      let btn = new Button()
      btn.Content <- "_Click me, please!"
      btn.Click.Add
         (fun _ -> MessageBox.Show("The Button has been Clicked and all is well.",
                                   this.Title) |>ignore)
      this.Content <- btn
end

(* From Chap 4 - FormatTheButton.cs *)
type FormatTheButton = class
   inherit Window //as base  
   
   val mutable runButton : Run   
   
   new () as this = {runButton=null} then
      this.Title <- "Format the Button"
      
      let btn = new Button()
      btn.HorizontalAlignment <- HorizontalAlignment.Center
      btn.VerticalAlignment <- VerticalAlignment.Center
      btn.Content <- "_Click me, please!"
      
      btn.MouseEnter.Add
         (fun _ -> this.runButton.Foreground <- Brushes.Red)
         
      btn.MouseLeave.Add
         (fun _ -> this.runButton.Foreground <- SystemColors.ControlTextBrush)

      this.Content <- btn
      
      let txtblk = new TextBlock();
      txtblk.FontSize <- 24.0;
      txtblk.TextAlignment <- TextAlignment.Center;
      btn.Content <- txtblk;
      txtblk.Inlines.Add(new Italic(new Run("Click")));
      txtblk.Inlines.Add(" the ");
      this.runButton <- new Run("button")
      txtblk.Inlines.Add(this.runButton);
      txtblk.Inlines.Add(new LineBreak());
      txtblk.Inlines.Add("to launch the ");
      txtblk.Inlines.Add(new Bold(new Run("rocket")));      
end

(* From Chap 4 - ImageTheButton.cs *)
type ImageTheButton = class
   inherit Window //as base  
      
   new () as this = {} then
      this.Title <- "Image the Button"
      
      let uri = new Uri("pack://application:,,/munch.png")
      let bitmap = new BitmapImage(uri)
      
      let img = new Image()
      img.Source <- bitmap
      img.Stretch <- Stretch.None

      let btn = new Button()
      btn.Content <- img
      btn.HorizontalAlignment <- HorizontalAlignment.Center
      btn.VerticalAlignment <- VerticalAlignment.Center
      
      this.Content <- btn
end

(* From Chap 4 - CommandTheButton.cs *)
type CommandTheButton = class
   inherit Window //as base  
      
   new () as this = {} then
      this.Title <- "Command the Button"

      let btn = new Button()
      btn.HorizontalAlignment <- HorizontalAlignment.Center
      btn.VerticalAlignment <- VerticalAlignment.Center
      btn.Command <- ApplicationCommands.Paste
      btn.Content <- ApplicationCommands.Paste.Text
      
      // Bind the command to the event handlers
      
      this.CommandBindings.Add
         (new CommandBinding
            (ApplicationCommands.Paste,
             // PasteOnExecute
             (fun _ _ -> this.Title <- Clipboard.GetText()), 
             // PasteCanExecute
             (fun _ (args:CanExecuteRoutedEventArgs) -> 
                args.CanExecute <- Clipboard.ContainsText()))) |>ignore
                
      this.Content <- btn

   override this.OnMouseDown (args:MouseButtonEventArgs) =
      base.OnMouseDown(args)
      this.Title <- "Command the Button"

end

(* From Chap 4 - ToggleTheButton.cs *)
type ToggleTheButton = class
   inherit Window //as base  
      
   new () as this = {} then
      this.Title <- "Toggle the Button"

      let btn = new ToggleButton()
      btn.Content <- "Can _Resize"      
      btn.HorizontalAlignment <- HorizontalAlignment.Center
      btn.VerticalAlignment <- VerticalAlignment.Center
      
      let checkTrue = new Nullable<bool>(true)
      let checkFalse = new Nullable<bool>(false)
      
      let isChecked () =
         match this.ResizeMode with
         | ResizeMode.CanResize ->  checkTrue
         | _ -> checkFalse
      
      let toggle e =
         match btn.IsChecked with
         | checkTrue -> this.ResizeMode <- ResizeMode.CanResize
         | _ -> this.ResizeMode <- ResizeMode.NoResize         
               
      btn.IsChecked <- isChecked()
      btn.Checked.Add(toggle)
      btn.Unchecked.Add(toggle)
      
      this.Content <- btn     

end

(* From Chap 4 - BindTheButton.cs *)
type BindTheButton = class
   inherit Window //as base  
      
   new () as this = {} then
      this.Title <- "Bind the Button"

      let btn = new ToggleButton()
      btn.Content <- "Can _Topmost"      
      btn.HorizontalAlignment <- HorizontalAlignment.Center
      btn.VerticalAlignment <- VerticalAlignment.Center
      btn.SetBinding(ToggleButton.IsCheckedProperty,"Topmost") |> ignore
      btn.DataContext <- this
      btn.SetBinding(ToggleButton.IsCheckedProperty,"ShowInTaskbar") |> ignore
      this.Content <- btn 
      let tip = new ToolTip()
      tip.Content <- "Toggle the button on to make " +
                     "the window topmost on the desktop"
      btn.ToolTip <- tip
end

(* From Chap 4 - UriDialog.cs *)
type UriDialog = class
   inherit Window //as base
   
   val txtbox : TextBox
      
   new () as this = {txtbox=new TextBox()} then
      this.Title <- "Enter a URI"
      this.ShowInTaskbar <- false
      this.SizeToContent <- SizeToContent.WidthAndHeight
      this.WindowStyle <- WindowStyle.ToolWindow
      this.WindowStartupLocation <- WindowStartupLocation.CenterOwner
      this.txtbox.Margin <- new Thickness(48.0)
      this.Content <- this.txtbox
      this.txtbox.Focus() |> ignore

   member this.Text
      with get() = this.txtbox.Text
      and set value =
         this.txtbox.Text <- value
         this.txtbox.SelectionStart <- this.txtbox.Text.Length

   override this.OnKeyDown args =
      if (args.Key = Key.Enter) then
         this.Close()
end

(* From Chap 4 - NavigateTheWeb.cs *)
type NavigateTheWeb = class
   inherit Window //as base  
   
   val frm : Frame
   new () as this = {frm = new Frame()} then
      this.Title <- "Navigate the Web"
      this.Content <- this.frm
      
      this.Loaded.Add
         (fun e ->
            let dlg = new UriDialog()
            dlg.Owner <- this
            dlg.Text <- "http://"
            dlg.ShowDialog() |> ignore
            try
               this.frm.Source <- new Uri(dlg.Text)
            with
               exn -> MessageBox.Show(exn.Message,this.Title) |> ignore)
            
end

(* From Chap 4 - EditSomeText.cs *)
type EditSomeText = class
   inherit Window //as base  
   
   val mutable strFileName : String
   val txtbox : TextBox
   
   new () as this = {
      
      strFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"test.txt");
      //strFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),"test.txt");
      //strFileName = @"C:\Users\mhatm\AppData\Local\test.txt";

      txtbox = new TextBox() } then

      this.Width <- 640.
      this.Height <- 480.
      this.Title <- "Edit Some Text"
      this.txtbox.AcceptsReturn <- true
      this.txtbox.TextWrapping <- TextWrapping.Wrap
      this.txtbox.VerticalScrollBarVisibility <- ScrollBarVisibility.Auto
      this.txtbox.KeyDown.Add
         ( fun args -> 
             match args.Key with
             | Key.F5 ->
                 this.txtbox.SelectedText <- DateTime.Now.ToString()
                 this.txtbox.CaretIndex <- this.txtbox.SelectionStart + 
                                           this.txtbox.SelectionLength
             | _ -> ())
      this.Content <- this.txtbox
      
      try
         this.txtbox.Text <- File.ReadAllText(this.strFileName)
      with
         exn -> MessageBox.Show("Error!") |>ignore
      
      this.txtbox.CaretIndex <- this.txtbox.Text.Length
      this.txtbox.Focus() |> ignore
   
   override this.OnClosing (args:CancelEventArgs) =
      try
         failwithf "Operation '%s' failed at time %O" this.strFileName DateTime.Now
         failwith "can't do it!"    // won't get here with the previous line
         Directory.CreateDirectory(Path.GetDirectoryName(this.strFileName))|>ignore
      with
         exc ->
            let result = MessageBox.Show("File could not be saved: " + exc.Message +
                                         "\nClose program anyway?", this.Title,
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Exclamation) 
            if (result = MessageBoxResult.No) then 
               args.Cancel <- true 
            else
               args.Cancel <- false
                                         
end

/// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/symbol-and-operator-reference/nullable-operators
//----------------------------------------------------------
// Nullable utilities for F# - Copied from FLinq sample code
let (=?!) (x: Nullable<'a>) (y:'a) =
   x.HasValue && x.Value = y

(* From Chap 4 - EditSomeRichText.cs *)   
type EditSomeRichText = class
   inherit Window //as base  
   
   val strFilter : string
   val txtbox : RichTextBox
   
   new () as this = {
      strFilter = "Document Files(*.xaml)|*.xaml|All files (*.*)|*.*";
      txtbox = new RichTextBox()} then

      this.Title <- "Edit Some Rich Text"
      this.txtbox.VerticalScrollBarVisibility <- ScrollBarVisibility.Auto 
      this.Content <- this.txtbox
      this.txtbox.Focus() |> ignore
   
   override this.OnPreviewTextInput (args:TextCompositionEventArgs ) =
      //if (args.ControlText.Length > 0 && (String.get args.ControlText 0) = '\x0F') then
      // '\x0F' (ASCII 15) is "SI" = "Shift In" (or CTRL-O)
      // ...so hit CTRL-O to OPEN the dialog
      if (args.ControlText.Length > 0 && (args.ControlText.[0]) = '\x0F') then
         let dlg = new OpenFileDialog()
         dlg.CheckFileExists <- true
         dlg.Filter <- this.strFilter
         
         // Using the newly defined operator =?! to deal with Nullable types
         if (dlg.ShowDialog(this) =?! true) then
            let flow = this.txtbox.Document
            let range = new TextRange(flow.ContentStart,flow.ContentEnd)
            try
               // The using idiom automatically closes the file connection
               // so we don't have to worry about it in exception handling
               using (new FileStream(dlg.FileName,FileMode.Open)) 
                  (fun strm ->
                     range.Load(strm,DataFormats.Xaml))
            with
               exn -> MessageBox.Show(exn.Message,this.Title) |> ignore
         args.Handled <- true
            
end




/// https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/compiler-directives
/// https://jyliao.blogspot.com/2007/11/learning-wpf-with-f-working-with.html
/// https://docs.microsoft.com/en-us/dotnet/framework/wpf/app-development/pack-uris-in-wpf


#if COMPILED
[<STAThread>]
do
    let app = new Application() in
    //app.Run(new ClickTheButton()) |> ignore
    //app.Run(new FormatTheButton()) |> ignore
    app.Run(new ImageTheButton()) |> ignore
    //app.Run(new CommandTheButton()) |> ignore
    //app.Run(new ToggleTheButton()) |> ignore
    //app.Run(new BindTheButton()) |> ignore
    //app.Run(new UriDialog()) |> ignore
    //app.Run(new NavigateTheWeb()) |> ignore
    //app.Run(new EditSomeText()) |> ignore
    //app.Run(new EditSomeRichText()) |> ignore

#endif


(*#if COMPILED
module BoilerPlateForForm =
    [<System.STAThread>]
    do ()
    do System.Windows.Forms.Application.Run()
#endif*)




