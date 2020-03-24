module FormDrawTree


open System
open System.Drawing
open System.Windows.Forms

//-------------------------------------------------------------------
/// https://www.c-sharpcorner.com/UploadFile/f5b919/how-to-draw-tree-structure-in-fsharp/

type 'N Tree =
    | Node of 'N Tree * 'N Tree
    | Leaf of 'N

// The definition of the tree
let tree =
    Node(
        Node(
            Leaf "C#",
            Node(Leaf "F#", Leaf "WPF")),
        Node(
            Node(Leaf "SILVERLIGHT", Leaf "WINFORM"),
            Leaf "WEBAPP"))


module DrawTreeModule =
    // A function for finding the maximum depth of a tree
    let getDepth A =
        let rec getDepthInner A B =
            match A with
            | Node (l, n) ->
                max
                    (getDepthInner l B+1.0F)
                    (getDepthInner n B+1.0F)
            | Leaf x -> B
        getDepthInner A 0.0F

    // Constants required for drawing the form
    let brush = new SolidBrush(Color.Black)
    let pen = new Pen(Color.Black)
    let font = new Font(FontFamily.GenericSerif, 8.0F)

    // A useful function for calculating the maximum number
    // of nodes at any given depth
    let raise2ToPower (x:float32) =
        Convert.ToSingle(Math.Pow(2.0, Convert.ToDouble(x)))

    /// where g is a graphics object and t is the tree to draw
    let drawTree (g:Graphics) t =
        // constants that relate to the size and position of the tree
        let center = g.ClipBounds.Width / 2.0F
        let maxWidth = 32.0F * raise2ToPower (getDepth t)

        // function for drawing a leaf node
        let drawLeaf (x:float32) (y:float32) v =
            let value = sprintf "%A" v
            let l = g.MeasureString(value, font)
            g.DrawString(value, font, brush, x - (l.Width / 2.0F), y)

        // draw a connector between the nodes when necessary
        let connectNodes (x:float32) y p =
            match p with
            | Some(px,py) -> g.DrawLine(pen, px, py, x, y)
            | None -> ()
    
        // the main function to walk the tree structure drawing the nodes as we go
        let rec drawTreeInner t d w p =
            let x = center - (maxWidth * w)
            let y = d * 32.0F
            connectNodes x y p
            match t with
            | Node (l, r) ->
                g.FillPie(brush, x-3.0F, y-3.0F, 7.0F, 7.0F, 0.0F, 360.0F)
                let d = (d + 1.0F)
                drawTreeInner l d (w + (1.0F / d)) (Some(x,y))
                drawTreeInner r d (w - (1.0F / d)) (Some(x,y))
            | Leaf v -> drawLeaf x y v
            
        drawTreeInner t 0.0F 0.0F None


#if COMPILED
let treeForm =
    let temp = new Form(WindowState=FormWindowState.Maximized, Text="Draw Tree")
    temp.Resize.Add(fun _ -> temp.Invalidate())
    temp.Paint.Add
        (fun e ->
            e.Graphics.Clip <- new Region(new Rectangle(0, 0, temp.Width, temp.Height))
            DrawTreeModule.drawTree e.Graphics tree
        )
    temp
Application.Run(treeForm)
#endif