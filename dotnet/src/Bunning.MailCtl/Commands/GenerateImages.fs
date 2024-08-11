namespace Bunning.MailCtl.Commands

open Argu
open Bunning.MailCtl.Args
open Bunning.MailCtl
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.TaskResult
open MailKit
open Microsoft.FSharp.Collections
open MimeKit
open MimeKit.Text

module GenerateImages =
    let private messageToImg (renderer: HtmlRenderer.T) (msg: MimeMessage) =
        msg.GetTextBody(format = TextFormat.Html)
        |> renderer.Render

    let private getMessages client renderer (msgIds: UniqueId seq) =
        msgIds
        |> List.ofSeq
        |> List.traverseTaskResultM (fun msgId -> Imap.getMessage client msgId >>= messageToImg renderer)


    let exec (args: ParseResults<GenerateImages>) =
        let host = args.GetResult(Host)
        let port = args.GetResult(Port, defaultValue = 0)
        let userName = args.GetResult(UserName)
        let password = args.GetResult(Password)

        let executablePath = "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome"
        let renderer = HtmlRenderer.T(executablePath)

        Imap.connect host port userName password
        >>= (fun client ->
            Imap.getMessageIds client
            >>= getMessages client renderer
            >>= (fun _ -> Imap.disconnect client))
