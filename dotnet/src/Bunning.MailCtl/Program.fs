namespace Bunning.MailCtl

open System.Threading.Tasks
open FSharpx

module Main =
    let private exitError e =
        eprintfn $"%s{e}"
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
            Commands.Process.exec args |> Task.WaitAll
            0
        | _ ->
            printfn $"%A{r}"
            0

    [<EntryPoint>]
    let main argv =
        Args.parseArgs argv |> Result.either execCmd exitError
