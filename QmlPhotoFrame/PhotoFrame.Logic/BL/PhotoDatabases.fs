namespace PhotoFrame.Logic.BL

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading


type PhotosListChangedEventArgs() =
    inherit EventArgs()


type PhotoEntry(filePath:string, hasBeenShownInThisSession:bool) =
    member val FilePath = filePath with get
    member val HasBeenShownInThisSession = hasBeenShownInThisSession
    // public string Filepath get
    // public bool HasBeenShownInThisSession get/set

    override this.Equals(o:obj) =
        (obj :? PhotoEntry) && (this.Equals(obj :> PhotoEntry))


    interface IEquatable<PhotoEntry> with
        member this.Equals (other:PhotoEntry) =
            other <> null &&
            this.FilePath = other.FilePath &&
                this.HasBeenShownInThisSession = other.HasBeenShownInThisSession

    override this.GetHashCode() =
        let mutable hashCode = -167777169
        hashCode <- hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.FilePath)
        hashCode


type PhotoDatabase(photosDirectoryPath:string) =
    
    let UpdateDatabase() =
        match IsStopped with
        | true -> ()
        | false ->
            let di = new DirectoryInfo(_photosDirectoryPath)
            let newList = di.EnumerateFiles("*.*").Where(fun fi -> 
                String.Equals(fi.Extension, ".jpg", StringComparison.InvariantCultureIgnoreCase)
                    || string.Equals(fi.Extension, ".png", StringComparison.InvariantCulureIgnoreCase))
                .OrderBy(fun fi -> fi.Name)
                .Select(fun fi -> new PhotoEntry(fi.FullName, false))
                .ToList()
            if (not AreListsEqual(newList, _photoEntries))
            then
                _photosEntries <- newList; RaisePhotosListChanged()
            else
                ()
                
    let _updateTimer = new Timer(fun o -> UpdateDatabase())
     
    let WaitAndUpdateDatabase() =
        _updateTimer.Change((int (TimeSpan.FromSeconds(2.).TotalMilliseconds)), Timeout.Infinite)
     
    let areEqual a b = Seq.fold (&&) true (Seq.zip a b |> Seq.map (fun (aa,bb) -> aa=bb))

    let AreListsEq<'T> listOne listTwo =
        match listOne, listTwo with
        | null, null -> true
        | null, _ -> false
        | _, null -> false
        | _, _ -> areEqual listOne listTwo



    (*let AreListsEqual<'T> listOne listTwo =
        if (listOne = null && listTwo = null)
        then true
        else if (listOne = null || listTwo = null)
        then false
        else if (listOne.Count <> listTwo.Count)
        then false
        else
            for i in [0..listOne.Count-1] do
                if not this.Equals(listOne[i], listTwo[i])
                then false
                else true*)


    let RaisePhotosListChanged() =
        do PhotosListChanged.Invoke(this, new PhotosListChangedEventArgs())

        do this.UpdateDatabase()

        member val _photoEntries:List<PhotoEntry> = null

        member val _photosDirectoryPath:string = photosDirectoryPath

        member val _fileSystemWatcher:FileSystemWatcher = new FileSystemWatcher(photosDirectoryPath)
        {
            NotifyFilter = NotifyFilters.LastWrite
                            | NotifyFilters.FileName
                            | NotifyFilters.DirectoryName
        }
        _fileSystemWatcher.Changed += (fun s,e -> WaitAndUpdateDatabase())
        _fileSystemWatcher.Created += (fun s,e -> WaitAndUpdateDatabase())
        _fileSystemWatcher.Deleted += (fun s,e -> WaitAndUpdateDatabase())
        _fileSystemWatcher.Renamed += (fun s,e -> WaitAndUpdateDatabase())

        _fileSystemWatcher.EnableRaisingEvents = true


        member val IsStopped:bool = false with get, set

        member val PhotosListChanged:EventHandler<PhotosListChangedEventArgs> = null

        member PhotoFiles:IReadOnlyList<PhotoEntry> = (function _photosEntries.AsReadOnly())

        member this.Stop() =
            IsStopped <- true

    










