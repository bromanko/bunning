namespace Bunning.MailCtl

module Main =
    [<EntryPoint>]
    let main argv =
        match Args.parseArgs argv with
        | Ok r ->
            match r with
            | [] ->
                Args.printUsage() |> printfn "%s"
                1
            | [Args.Version] ->
                Commands.Version.exec() |> printfn "%s"
                0
            | _ ->
                printfn $"%A{r}"
                0
        | Error e ->
            eprintfn $"%s{e}"
            1