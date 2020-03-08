module WinFormsMain


open System
open System.IO
open System.Drawing
open System.Windows
open System.Windows.Forms
open MyDemo
open System.Windows.Resources



let form = new Form(Visible=true, Text="A Web Browser control")
let container = new TableLayoutPanel(ColumnCount=4, RowCount=4)
let label = new Label(Text="Address:")
let address = new TextBox(Margin=new Padding(5, 2, 30, 2))
let toolbar = new ToolStrip(MinimumSize=new System.Drawing.Size(150,40), Stretch=true)
let content = new WebBrowser()

let font' = new Font(FontFamily.GenericSansSerif, (float32) 20, FontStyle.Bold)
let back = new ToolStripButton(Text="Prev", Font=font')
let forward = new ToolStripButton("Next", Font=font')
let font = new Font(FontFamily.GenericSansSerif, (float32) 40, FontStyle.Bold)
let mutable blankLabel = new Label(Text=" ")
blankLabel.Margin <- new Padding(1) //(left, top, right, bottom)
let btn1 = new ToolStripButton(Text="1", Font=font)
let btn2 = new ToolStripButton(Text="2", Font=font)
let btn3 = new ToolStripButton(Text="3", Font=font)
let btn4 = new ToolStripButton(Text="4", Font=font)
let btn5 = new ToolStripButton(Text="5", Font=font)
let btn6 = new ToolStripButton(Text="6", Font=font)
let btn7 = new ToolStripButton(Text="7", Font=font)

label.TextAlign <- ContentAlignment.MiddleRight

form.Width <- 840
form.Height <- 680
form.Left <- 1000
form.Top <- 400

container.Dock <- DockStyle.Fill
address.Dock <- DockStyle.Fill
content.Dock <- DockStyle.Fill

toolbar.Items.Add(back) |> ignore
toolbar.Items.Add(forward) |> ignore
toolbar.Items.Add(btn1) |> ignore
toolbar.Items.Add(btn2) |> ignore
toolbar.Items.Add(btn3) |> ignore
toolbar.Items.Add(btn4) |> ignore
toolbar.Items.Add(btn5) |> ignore
toolbar.Items.Add(btn6) |> ignore
toolbar.Items.Add(btn7) |> ignore


let LoadImage imageName =
    let s:string = System.IO.Packaging.PackUriHelper.UriSchemePack
    let uri:Uri = new Uri("pack://application:,,,/images/"+imageName, UriKind.RelativeOrAbsolute)
    let info:StreamResourceInfo = System.Windows.Application.GetResourceStream(uri) //GetContentStream(uri)
    let img = System.Drawing.Image.FromStream(info.Stream)
    img     // return the Image


(*image.Source <- bitmap
image.Stretch <- Stretch.None*)
(*content.BackgroundImage <- myImage
content.BackgroundImageLayout <- ImageLayout.Stretch*)
(*container.BackgroundImage <- myImage
container.BackgroundImageLayout <- ImageLayout.Stretch*)
(*blankLabel.Image <- myImage
blankLabel.ImageAlign <- ContentAlignment.TopRight*)
blankLabel.BackgroundImage <- (LoadImage "munch.png")
blankLabel.BackgroundImageLayout <- ImageLayout.Stretch
blankLabel.Height <- 180
blankLabel.Dock <- DockStyle.Fill

form.Controls.Add(container)
container.Controls.Add(blankLabel, 2, 0)
container.Controls.Add(label, 0, 1)
container.Controls.Add(address, 1, 1)
container.Controls.Add(toolbar, 0, 2)
container.Controls.Add(content, 0, 3)

container.SetColumnSpan(blankLabel, 2)
container.SetColumnSpan(address, 3)
container.SetColumnSpan(toolbar, 4)
container.SetColumnSpan(content, 4)
content.Refresh()

back.Click.Add(fun _ -> content.GoBack() |> ignore)
forward.Click.Add(fun _ -> content.GoForward() |> ignore)

address.KeyDown.Add(fun e ->
    if e.KeyCode = Keys.Enter then
        try content.Url <- System.Uri(address.Text)
        with _ -> ())


/// CLICK NUMBERED BUTTONS TO LAUNCH DEMO SECTIONS
btn1.Click.Add(fun _ -> MyDemo.demo 1 |> ignore)
btn2.Click.Add(fun _ -> MyDemo.demo 2 |> ignore)
btn3.Click.Add(fun _ -> MyDemo.demo 3 |> ignore)
btn4.Click.Add(fun _ -> MyDemo.demo 4 |> ignore)


 
form.Show()
#if COMPILED
[<STAThread()>]
Application.Run(form)
#endif
