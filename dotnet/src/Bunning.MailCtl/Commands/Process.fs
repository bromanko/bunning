namespace Bunning.MailCtl.Commands

open Argu
open Bunning.MailCtl.Args
open Bunning.MailCtl
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.TaskResult
open MailKit

module Process =
    let private getMessages client (msgIds: UniqueId seq) =
        msgIds |> Seq.iter (printfn "%A")
        TaskResult.ok ()

    let exec (args: ParseResults<ProcessArgs>) =
        let host = args.GetResult(Host)
        let port = args.GetResult(Port, defaultValue = 0)
        let userName = args.GetResult(UserName)
        let password = args.GetResult(Password)

        Imap.connect host port userName password
        >>= (fun client -> Imap.getMessageIds client >>= getMessages client)
