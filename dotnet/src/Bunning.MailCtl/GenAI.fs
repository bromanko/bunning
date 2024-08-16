namespace Bunning.MailCtl

open Bunning.MailCtl.OpenAI
open Bunning.MailCtl.OpenAI.Completions
open FSharpx.Collections
open FsToolkit.ErrorHandling

module GenAI =
    let private mkPrompt = Message.System { Content = ""; Name = None }

    let private mkProcessImageReq () =
        { Model = "gpt-4o-mini"
          Messages = NonEmptyList.create mkPrompt [] }

    [<RequireQualifiedAccess>]
    type T(openAIKey) =
        let client =
            Config(
                { Endpoint = "https://api.openai.com/v1"
                  ApiKey = openAIKey },
                HttpRequester()
            )

        member this.ProcessImage() =
            task {
                try
                    return!
                        client
                        |> completions
                        |> create (mkProcessImageReq ())
                        |> Task.ignore
                        |> Task.map Ok
                with ex ->
                    return Error ex
            }
