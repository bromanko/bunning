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

module Process =
    let private messageToImg (msg: MimeMessage) =
        printfn $"%A{msg.GetTextBody(format = TextFormat.Html)}"
        TaskResult.ok ()

    let private getMessages client (msgIds: UniqueId seq) =
        msgIds
        |> List.ofSeq
        |> List.traverseTaskResultM (fun msgId -> Imap.getMessage client msgId >>= messageToImg)


    let exec (args: ParseResults<ProcessArgs>) =
        let host = args.GetResult(Host)
        let port = args.GetResult(Port, defaultValue = 0)
        let userName = args.GetResult(UserName)
        let password = args.GetResult(Password)

        Imap.connect host port userName password
        >>= (fun client ->
            Imap.getMessageIds client
            >>= getMessages client
            >>= (fun _ -> Imap.disconnect client))
