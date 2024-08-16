namespace Bunning.MailCtl.Commands

open Argu
open Bunning.MailCtl.Args
open FsToolkit.ErrorHandling

module ParseImages =
    let exec (args: ParseResults<ParseImages>) = TaskResult.FromResult <| Ok()
