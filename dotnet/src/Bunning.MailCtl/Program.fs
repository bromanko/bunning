namespace Bunning.MailCtl

open FsToolkit.ErrorHandling

module Main =
    let private exitError (e: exn) =
        eprintfn $"%A{e}"
        1

    let private execCmd r =
        match r with
        | [] ->
            Args.printUsage () |> printfn "%s"
            1
        | [ Args.Version ] ->
            Commands.Version.exec () |> printfn "%s"
            0
        | [ Args.Process args ] ->
            Commands.Process.exec args
            |> TaskResult.foldResult (fun _ -> 0) exitError
            |> Async.AwaitTask
            |> Async.RunSynchronously
        | _ ->
            printfn $"%A{r}"
            0

    [<EntryPoint>]
    let main argv =
        Args.parseArgs argv |> Result.either execCmd exitError
