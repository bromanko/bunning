namespace Bunning.MailCtl

open Argu

module Args =
    type GenerateImages =
        | [<AltCommandLine("-h"); Mandatory>] Host of host: string
        | [<AltCommandLine("-p"); >] Port of port: int
        | [<AltCommandLine("-u"); Mandatory>] UserName of userName: string
        | [<AltCommandLine("-w"); Mandatory>] Password of password: string
        | MessageIds of message_id: string list

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Host _ -> "Host of the IMAP server to connect to."
                | Port _ -> "Port of the IMAP server to connect to."
                | UserName _ -> "User name to use when connecting to the IMAP server."
                | Password _ -> "Password to use when connecting to the IMAP server."
                | MessageIds _ -> "Message ids to generate images of. If none are specified, all messages will be processed."

    and MailCtlArgs =
        | Version
        | [<CliPrefix(CliPrefix.None)>] Process of ParseResults<GenerateImages>

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
            Result.Error e

    let printUsage =
        parser.PrintUsage