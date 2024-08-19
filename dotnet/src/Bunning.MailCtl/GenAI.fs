namespace Bunning.MailCtl

open Bunning.MailCtl.OpenAI
open Bunning.MailCtl.OpenAI.Completions
open FsHttp
open FsToolkit.ErrorHandling

module GenAI =
    let private promptContent = """
I will provide you with an image of a marketing email.
You will check if the image contains a promotion code and will extract it.
If there are restrictions on the promotion code, you will extract the restrictions.
You will return the extracted data as json.
"""
    let private mkPrompt = Message.System { Content = promptContent; Name = None }

    let private mkProcessImageReq () =
        { Model = "gpt-4o-mini"
          Messages = [mkPrompt] }

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
                    Fsi.enableDebugLogs()
                    return!
                        client
                        |> completions
                        |> create (mkProcessImageReq ())
                        |> Task.map (fun x ->
                            printfn $"%A{x}"
                            ()
                            )
                        |> Task.map Ok
                with ex ->
                    return Error ex
            }
