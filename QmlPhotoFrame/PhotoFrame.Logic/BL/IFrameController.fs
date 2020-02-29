namespace PhotoFrame.Logic.BL


open System
open System.Drawing



type CurrentPhotoChangedEventArgs() =
    inherit EventArgs()

type TimerValueChangedEventArgs() =
    inherit EventArgs()

type ColorizeInfo(hue:double, saturation:double, lightness:double) =
    member val Hue = hue with get
    member val Saturation = saturation with get
    member val Lightness = lightness with get


type IFrameController =
    abstract member CurrentPhotoChanged : EventHandler<CurrentPhotoChangedEventArgs>
    abstract member TimerValueChanged : EventHandler<TimerValueChangedEventArgs>

    abstract member GetNextBorderColor() : Color
    abstract member GetNextColorizeInfo() : ColorizeInfo

    abstract member GetNextViewSwitchType() : ViewSwitchType
    abstract member CurrentPhoto : string with get
    abstract member TimerValue : int with get
    abstract member GetNextViewType() : ViewType

    abstract member Start()
    abstract member Stop()




