namespace Bunning.MailCtl.Commands

open Argu
open Bunning.MailCtl.Args
open Bunning.MailCtl

module Process =
    let exec (args: ParseResults<ProcessArgs>) =
        task {
            let host = args.GetResult(Host)
            let port = args.GetResult(Port, defaultValue = 0)
            let userName = args.GetResult(UserName)
            let password = args.GetResult(Password)

            match! Imap.getMessageIds host port userName password with
            | Ok ids ->
                printfn $"%A{ids}"
                return Ok()
            | Error e -> return Error e
        }
