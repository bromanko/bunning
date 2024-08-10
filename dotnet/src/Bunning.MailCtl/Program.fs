namespace Bunning.MailCtl

open FsToolkit.ErrorHandling
open FSharpx

module Main =
    let private exitSuccess = konst 0

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
            |> TaskResult.foldResult exitSuccess exitError
            |> Async.AwaitTask
            |> Async.RunSynchronously
        | _ ->
            printfn $"%A{r}"
            0

    [<EntryPoint>]
    let main argv =
        Args.parseArgs argv |> Result.either execCmd exitError
