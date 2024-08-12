namespace Bunning.MailCtl.Commands

open System
open System.IO
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
    let private messageToImg (renderer: HtmlRenderer.T) outPath (msg: MimeMessage) =
        let html = msg.GetTextBody(format = TextFormat.Html)
        let outPath = Path.Combine(outPath, msg.MessageId + ".png")
        renderer.Render outPath html

    let private getMessages (imap: Imap.T) renderer outPath (msgIds: UniqueId seq) =
        msgIds
        |> List.ofSeq
        |> List.traverseTaskResultM (fun msgId -> imap.GetMessage msgId >>= messageToImg renderer outPath)


    let exec (args: ParseResults<GenerateImages>) =
        let host = args.GetResult(Host)
        let port = args.GetResult(Port, defaultValue = 0)
        let userName = args.GetResult(UserName)
        let password = args.GetResult(Password)
        let imap = new Imap.T()

        let executablePath =
            args.GetResult(Browser_Path, defaultValue = HtmlRenderer.defaultExecutablePath ())

        let renderer = new HtmlRenderer.T(executablePath)
        let outPath = args.GetResult(Output)

        imap.Connect host port userName password
        >>= imap.GetMessageIds
        >>= getMessages imap renderer outPath
        >>= (fun _ ->
            let r = imap.Disconnect()
            (imap :> IDisposable).Dispose()
            (renderer :> IDisposable).Dispose()
            r)
