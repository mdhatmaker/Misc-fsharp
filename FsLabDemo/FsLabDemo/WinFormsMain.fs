module WinFormsMain

/// https://www.c-sharpcorner.com/technologies/f-sharp


open System
open System.IO
open System.Drawing
open System.Windows
open System.Windows.Forms
open System.Windows.Resources
open MyDemoCharts
open MyLiveCharts
open FSharp.Charting.ChartTypes


//-------------------------------------------------------------------
module FormModule =
    let form = new Form(Visible=true, Text="FSharp.Charting Demo")
    let container = new TableLayoutPanel(ColumnCount=4, RowCount=5)
    let label = new Label(Text="Address:")
    let address = new TextBox(Margin=new Padding(5, 2, 30, 2))
    let toolbarA = new ToolStrip(MinimumSize=new System.Drawing.Size(150,40), Stretch=true)
    let toolbarB = new ToolStrip(MinimumSize=new System.Drawing.Size(150,40), Stretch=true)
    let content = new WebBrowser()
    let picbox = new PictureBox(BackColor=Color.LightSteelBlue)
    let chartPanel = new Panel(BackColor=Color.DarkBlue, Dock=DockStyle.Fill)

    let font' = new Font(FontFamily.GenericSansSerif, (float32) 20, FontStyle.Bold)
    let back = new ToolStripButton(Text="Prev", Font=font')
    let forward = new ToolStripButton("Next", Font=font')
    let font = new Font(FontFamily.GenericSansSerif, (float32) 40, FontStyle.Bold)
    let mutable blankLabel = new Label(Text=" ")
    blankLabel.Margin <- new Padding(1) //(left, top, right, bottom)
    (*let btn1 = new ToolStripButton(Text="1", Font=font)
    let btn2 = new ToolStripButton(Text="2", Font=font)
    let btn3 = new ToolStripButton(Text="3", Font=font)
    let btn4 = new ToolStripButton(Text="4", Font=font)
    let btn5 = new ToolStripButton(Text="5", Font=font)
    let btn6 = new ToolStripButton(Text="6", Font=font)
    let btn7 = new ToolStripButton(Text="7", Font=font)*)

    let btnsA = [ for i in 1..10 do yield new ToolStripButton(Text=i.ToString(), Font=font) ]

    let btnsB = [ for i in 1..12 do yield new ToolStripButton(Text=(i+10).ToString(), Font=font) ]

    
        
    label.TextAlign <- ContentAlignment.MiddleRight

    form.Width <- 1080
    form.Height <- 680
    form.Left <- 1000
    form.Top <- 400

    picbox.Dock <- DockStyle.Fill
    container.Dock <- DockStyle.Fill
    address.Dock <- DockStyle.Fill
    content.Dock <- DockStyle.Fill

    toolbarA.Items.Add(back) |> ignore
    toolbarA.Items.Add(forward) |> ignore
    (*toolbarA.Items.Add(btn1) |> ignore
    toolbarA.Items.Add(btn2) |> ignore
    toolbarA.Items.Add(btn3) |> ignore
    toolbarA.Items.Add(btn4) |> ignore
    toolbarA.Items.Add(btn5) |> ignore
    toolbarA.Items.Add(btn6) |> ignore
    toolbarA.Items.Add(btn7) |> ignore*)


    let buttonClickedA (txt:string) =
        printfn "buttonClickedA: '%s'" txt
        let chart = MyDemoCharts.demo txt
        chartPanel.Controls.Clear()
        // these demo charts are displayed individually in popup windows
        (*let chartControl = new ChartControl(chart)
        chartPanel.Controls.Add(chartControl)
        chartControl.Dock <- DockStyle.Fill*)

    /// CLICK NUMBERED BUTTONS TO LAUNCH DEMO SECTIONS
    (*btn1.Click.Add(fun _ -> MyDemoCharts.demo 1 |> ignore)
    btn2.Click.Add(fun _ -> MyDemoCharts.demo 2 |> ignore)
    btn3.Click.Add(fun _ -> MyDemoCharts.demo 3 |> ignore)
    btn4.Click.Add(fun _ -> MyDemoCharts.demo 4 |> ignore)
    btn5.Click.Add(fun _ -> MyDemoCharts.demo 5 |> ignore)
    btn6.Click.Add(fun _ -> MyDemoCharts.demo 5 |> ignore)*)
    for a in btnsA do
        toolbarA.Items.Add(a) |> ignore
        a.Click.Add(fun _ -> buttonClickedA a.Text |> ignore)

    let buttonClickedB (txt:string) =
        printfn "buttonClickedB: '%s'" txt
        let chart = MyLiveCharts.demo txt
        chartPanel.Controls.Clear()
        // these live charts are displayed in our UI chartPanel
        let chartControl = new ChartControl(chart)
        chartPanel.Controls.Add(chartControl)
        chartControl.Dock <- DockStyle.Fill

    for b in btnsB do
        toolbarB.Items.Add(b) |> ignore
        b.Click.Add(fun _ -> buttonClickedB b.Text |> ignore)


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
    container.Controls.Add(toolbarA, 0, 2)
    container.Controls.Add(toolbarB, 0, 3)
    container.Controls.Add(content, 0, 4)
    container.Controls.Add(picbox, 2, 4)
    container.Controls.Add(chartPanel, 3, 4)

    container.SetColumnSpan(blankLabel, 2)
    container.SetColumnSpan(address, 3)
    container.SetColumnSpan(toolbarA, 4)
    container.SetColumnSpan(toolbarB, 4)
    container.SetColumnSpan(content, 2)
    container.SetColumnSpan(picbox, 1)
    container.SetColumnSpan(chartPanel, 1)

    content.Refresh()

    back.Click.Add(fun _ -> content.GoBack() |> ignore)
    forward.Click.Add(fun _ -> content.GoForward() |> ignore)

    address.KeyDown.Add(fun e ->
        if e.KeyCode = Keys.Enter then
            try content.Url <- System.Uri(address.Text)
            with _ -> ())




    
 

//-------------------------------------------------------------------

#if COMPILED
// In case you're interested in Single-Threaded Apartment (STA):
// https://devblogs.microsoft.com/cbrumme/apartments-and-pumping-in-the-clr/
[<STAThread()>]
Application.Run(FormModule.form)
#else
FormModule.form.Show()
#endif
