open System
open System.Windows 
open System.Windows.Controls 
open System.Windows.Shapes 
open System.Windows.Media 

[<Measure>] 
type deg
[<Measure>] 
type rad = 1

type Command =
    | Fwd of int
    | Left of int
    | Right of int
    | PenUp
    | PenDown
    | Repeat of int * Command list

let Repeat n cmds = Repeat(n,cmds)  // for syntactic elegance of EDSL

let MillisecondsToSleepBetweenCommands = 30
let WIDTH = 400.0
let HEIGHT = 300.0
let PI = System.Math.PI 
let deg2rad (d:float<deg>) : float<rad> = d * PI / 180.0<deg>

type MyWindow(theDrawing) as this =
    inherit Window(Title="Fun LOGO drawing")
    
    let canvas = new Canvas(Width=WIDTH, Height=HEIGHT)
    let mutable curX = WIDTH / 2.0
    let mutable curY = HEIGHT / 2.0
    let mutable curT = 0.0<deg>
    let mutable isPenDown = true
    let turtle = 
        // a right-pointing triangle centered about the origin
        let points = PointCollection [  Point( -5.0, -10.0)
                                        Point(  5.0,   0.0)
                                        Point( -5.0,  10.0)
                                        Point( -5.0, -10.0)  ]
        points.Freeze()
        let poly = new Polygon(Points = points, Stroke = Brushes.Green)
        canvas.Children.Add poly |> ignore
        poly
    let update() = async {
        Canvas.SetTop(turtle, curY)
        Canvas.SetLeft(turtle, curX)
        turtle.RenderTransform <- new RotateTransform(Angle=float curT)
        do! Async.Sleep MillisecondsToSleepBetweenCommands
        }
    do
        Canvas.SetTop(turtle, curY)
        Canvas.SetTop(turtle, curX)
        this.Content <- canvas
        this.SizeToContent <- SizeToContent.WidthAndHeight 
        this.Loaded.Add (fun _ ->
            async {
                do! Async.Sleep 200
                for cmd in theDrawing do
                    do! this.Execute(cmd)
            } |> Async.StartImmediate 
        )

    /// <summary>
    /// Execute a single LOGO command and update the screen
    /// </summary>
    /// <param name="command">The Command to be executed</param>
    member this.Execute command = async {
        match command with
        | PenUp ->
            isPenDown <- false
        | PenDown ->
            isPenDown <- true
        | Fwd n -> 
            let t = deg2rad curT
            let newX = curX + float n * cos t
            let newY = curY + float n * sin t
            if isPenDown then 
                let line = new Line(X1=curX, X2=newX, Y1=curY, Y2=newY, Stroke = Brushes.Black)
                canvas.Children.Add line |> ignore
            curX <- newX
            curY <- newY
            do! update()
        | Left t ->
            curT <- curT - float t * 1.0<deg>
            do! update()
        | Right t ->
            curT <- curT + float t * 1.0<deg>
            do! update()
        | Repeat(n,cmds) ->
            for _ in 1..n do
                for cmd in cmds do
                    do! this.Execute cmd
        }

let EYE = Repeat 1 [ PenDown; Repeat 8 [ Fwd 20; Right 45 ]; PenUp ]

//let DRAWING = [ // go to good start location
//                PenUp
//                Left 135
//                Fwd 80
//                Right 135
//                // left eye
//                EYE
//                // move right
//                Fwd 80
//                // right eye
//                EYE
//                // go to mouth corner
//                Fwd 60
//                Right 90
//                Fwd 90
//                // smile
//                PenDown
//                Right 45
//                Fwd 60
//                Right 45
//                Fwd 100
//                Right 45
//                Fwd 60
//                // circle around face
//                PenUp
//                Left 135
//                Fwd 80
//                Left 90
//                Fwd 30
//                PenDown
//                Repeat 8 [
//                    Fwd 100
//                    Left 45
//                ]
//              ]

//let koch1 = [ Fwd 243; Right 120; Fwd 243; Right 120; Fwd 243 ]
//
//let kochChange(drawing) = seq {
//    for cmd in drawing do
//        match cmd with
//        | Fwd n -> 
//            let x = n/3
//            yield Fwd x
//            yield Left 60
//            yield Fwd x
//            yield Right 120
//            yield Fwd x
//            yield Left 60
//            yield Fwd x
//        | c -> yield c }
//
//let DRAWING = koch1 |> kochChange |> kochChange |> kochChange |> Seq.append [PenUp; Left 145; Fwd 120; Right 145; PenDown]

let rec Dragon1 size level = seq {
    if level > 0 then
        yield! Dragon (size*707/1000) (level-1)
        yield Left 90
        yield! Dragon1 (size*707/1000) (level-1) 
    else
        yield Fwd size
    }
and Dragon size level = seq {
    if level > 0 then
        yield! Dragon (size*707/1000) (level-1)
        yield Right 90
        yield! Dragon1 (size*707/1000) (level-1)
    else
        yield Fwd size 
    }

let DRAWING = Dragon 800 14

[<STAThread>]
do
    let app = new Application()
    app.Run(new MyWindow(DRAWING)) |> ignore