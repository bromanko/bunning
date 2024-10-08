namespace Bunning.MailCtl

open System.Threading.Tasks

module OpenAI =
    type Config(apiConfig: ApiConfig, httpRequester: IHttpRequester) =
        member this.ApiConfig = apiConfig
        member this.HttpRequester = httpRequester

    type ErrorResponse =
        { Error:
            {| Message: string
               Type: string
               Param: string
               Code: string |} }

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
              Messages: List<Message> }

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

        let create (request: CreateRequest) (cfg: Config) : Task<Result<CreateResponse, ErrorResponse>> =
            cfg.HttpRequester.postRequest<CreateRequest, ErrorResponse, CreateResponse> cfg.ApiConfig request
