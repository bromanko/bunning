namespace Bunning.MailCtl

open Argu

module Args =
    type GenerateImages =
        | [<AltCommandLine("-h"); Mandatory>] Host of host: string
        | [<AltCommandLine("-p")>] Port of port: int
        | [<AltCommandLine("-u"); Mandatory>] UserName of user_name: string
        | [<AltCommandLine("-w"); Mandatory>] Password of password: string
        | Message_Id of message_id: string list
        | Browser_Path of browser_path: string
        | [<AltCommandLine("-o"); Mandatory>] Output of output_path: string

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Host _ -> "Host of the IMAP server to connect to."
                | Port _ -> "Port of the IMAP server to connect to."
                | UserName _ -> "User name to use when connecting to the IMAP server."
                | Password _ -> "Password to use when connecting to the IMAP server."
                | Message_Id _ ->
                    "Message ids to generate images of. If none are specified, all messages will be processed."
                | Browser_Path _ ->
                    $"Path to the browser executable to use for rendering HTML. Defaults to ${HtmlRenderer.defaultExecutablePath ()}"
                | Output _ -> "Path to output the generated images."

    and ParseImages =
        | Image of image: string list
        | [<Mandatory>] OpenAI_Key of open_ai_key: string

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Image _ -> "Path to an image to be parsed."
                | OpenAI_Key _ -> "OpenAI API key."

    and MailCtlArgs =
        | Version
        | [<CliPrefix(CliPrefix.None)>] Generate_Images of ParseResults<GenerateImages>
        | [<CliPrefix(CliPrefix.None)>] Parse_Images of ParseResults<ParseImages>

        interface IArgParserTemplate with
            member s.Usage =
                match s with
                | Version -> "Display version information."
                | Generate_Images _ -> "Generate image of mails within an IMAP mailbox."
                | Parse_Images _ -> "Parse images to structured data."


    let private parser = ArgumentParser.Create<MailCtlArgs>(programName = "mailctl")

    let parseArgs argv =
        try
            let results = parser.ParseCommandLine(argv)
            results.GetAllResults() |> Result.Ok
        with :? ArguParseException as e ->
            Result.Error e

    let printUsage = parser.PrintUsage
