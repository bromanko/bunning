namespace Bunning.MailCtl

open System.Threading.Tasks
open FSharpx.Collections

module OpenAI =
    type Config(apiConfig: ApiConfig, httpRequester: IHttpRequester) =
        member this.ApiConfig = apiConfig
        member this.HttpRequester = httpRequester

    module Completions =
        type SystemMessage =
            { Content: string
              Name: string option }

            member this.Role = "system"

        type UserMessage =
            { Content: string
              Name: string option }

            member this.Role = "user"

        type Message =
            | System of SystemMessage
            | User of UserMessage

        type CreateRequest =
            { Model: string
              Messages: NonEmptyList<Message> }

        type ResponseFormat = JsonSchema of string // TODO: Switch to a JsonSchema object

        type ChoiceMessage =
            { Content: string option
              Refusal: string option }

        type Choice =
            { Index: int
              Message: ChoiceMessage
              Created: int }

        type CreateResponse = { Id: string; Choices: Choice list }

        let completions (cfg: Config) : Config =
            Config(
                { cfg.ApiConfig with
                    Endpoint = cfg.ApiConfig.Endpoint + "/chat/completions" },
                cfg.HttpRequester
            )

        let create (request: CreateRequest) (cfg: Config) : Task<CreateResponse> =
            cfg.HttpRequester.postRequest<CreateRequest, CreateResponse> cfg.ApiConfig request
