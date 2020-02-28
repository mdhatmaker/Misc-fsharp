namespace Features


open System
open System.Threading.Tasks


type AsyncAwaitModel() =

    //public async Task<string> RunAsyncTask(string message)
    member this.RunAsyncTask(message:string) =
        //await Task.Delay(TimeSpan.FromSeconds(2));
        message
