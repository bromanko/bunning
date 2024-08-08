namespace Bunning.MailCtl

open Argu

module Args =
    type ProcessArgs =
        | [<AltCommandLine("-h")>] Host of host: string
        | [<AltCommandLine("-p")>] Port of port: int

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Host _ -> "Host of the IMAP server to connect to."
                | Port _ -> "Port of the IMAP server to connect to"

    and MailCtlArgs =
        | Version
        | [<CliPrefix(CliPrefix.None)>] Process of ParseResults<ProcessArgs>

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Version -> "Display version information."
                | Process _ -> "Process mails within an IMAP mailbox."


    let private parser = ArgumentParser.Create<MailCtlArgs>(programName = "mailctl")

    let parseArgs argv =
        try
            let results = parser.ParseCommandLine(argv)
            results.GetAllResults() |> Result.Ok
        with :? ArguParseException as e ->
            Result.Error e.Message

    let printUsage =
        parser.PrintUsage