module WinFormsWebBrowser

/// https://www.c-sharpcorner.com/UploadFile/f5b919/webbrowser-control-in-fsharp/

open System
open System.Drawing
open System.Windows.Forms

open TpWorldBank


let form = new Form(Visible=true, Text="A Web Browser control")
let container = new TableLayoutPanel(ColumnCount=2, RowCount=3)
let label = new Label(Text="Address:")
let address = new TextBox()
let toolbar = new ToolStrip()
let content = new WebBrowser()
let back = new ToolStripButton("Previous")
let forward = new ToolStripButton("Next")
let btn1 = new ToolStripButton("1")

label.TextAlign <- ContentAlignment.MiddleRight

form.Width <- 640
form.Height <- 480
form.Left <- 1024
form.Top <- 480

container.Dock <- DockStyle.Fill
address.Dock <- DockStyle.Fill
content.Dock <- DockStyle.Fill

toolbar.Items.Add(back) |> ignore
toolbar.Items.Add(forward) |> ignore
toolbar.Items.Add(btn1) |> ignore

form.Controls.Add(container)
container.Controls.Add(label, 0, 0)
container.Controls.Add(address, 1, 0)
container.Controls.Add(toolbar, 0, 1)
container.Controls.Add(content, 0, 2)
container.SetColumnSpan(toolbar, 2)
container.SetColumnSpan(content, 2)
content.Refresh()

back.Click.Add(fun _ -> content.GoBack() |> ignore)
forward.Click.Add(fun _ -> content.GoForward() |> ignore)

btn1.Click.Add(fun _ -> TpWorldBank.demo() |> ignore)

address.KeyDown.Add(fun e ->
    if e.KeyCode = Keys.Enter then
        try content.Url <- System.Uri(address.Text)
        with _ -> ())


 
form.Show()
#if COMPILED
[<STAThread()>]
Application.Run(form)
#endif