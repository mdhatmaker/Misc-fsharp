namespace PhotoFrame.Logic.BL

open System
open System.Linq
open System.Threading
open System.Drawing
open PhotoFrame.Logic.Config


type FrameController(frameConfig : IFrameConfig, uiDispatchDelegate : UiDispatchDelegate) =
    
    let _frameConfig = frameConfig
    let _uiDispatchDelegate = uiDispatchDelegate
    
    let _photosDatabase = new PhotoDatabase(_frameConfig.PhotosPath)
    let _photosDatabase.PhotosListChanged = (fun (s,e) -> 
        let nextPhoto = CalculateNextPhoto()
        _uiDispatchDelegate.Invoke(fun () -> 
            CurrentPhoto <- nextPhoto
        )
    
    
    //interface IFrameController with
    

    member this.Start() =
        _timerValue <- TimeSpan.Zero
        _photoSwitchTimer.Change(TimeSpan.Zero, TimeSpan.FromSeconds(1.))


    member this.Stop() =
        _photoSwitchTimer.Change(Timeout.Infinite, Timeout.Infinite)

    let mutable _currentPhoto = ""

    member.this CurrentPhoto
    with get() = _currentPhoto
    and set(value) =
        _currentPhoto <- value
        RaiseCurrentPhotoChanged()

    member.this 



