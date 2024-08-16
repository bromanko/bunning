namespace Bunning.MailCtl.Commands

open Argu
open Bunning.MailCtl.Args
open Bunning.MailCtl
open FsToolkit.ErrorHandling.Operator.TaskResult

module ParseImages =
    let exec (args: ParseResults<ParseImages>) =
        let openAIKey = args.GetResult(OpenAI_Key)
        let client = GenAI.T(openAIKey)

        client.ProcessImage()
