namespace Bunning.MailCtl

open System.Text.Json.Serialization
open System.Threading
open System.Threading.Tasks
open FsHttp
open FsToolkit.ErrorHandling.Operator.Task
open FSharp.SystemTextJson

type ApiConfig = { Endpoint: string; ApiKey: string }

type IHttpRequester =
    abstract member postRequest<'T, 'R>: cfg: ApiConfig -> data: 'T -> Task<'R>
    abstract member getRequest<'R>: cfg: ApiConfig -> Task<'R>

type HttpRequester() =
    let jsonSerializerOptions = JsonFSharpOptions.Default().ToJsonSerializerOptions()

    interface IHttpRequester with
        member this.postRequest<'T, 'R>(cfg: ApiConfig) (data: 'T) =
            http {
                POST cfg.Endpoint
                AuthorizationBearer cfg.ApiKey
                Accept "application/json"
                CacheControl "no-cache"
                body
                ContentType "application/json"
                jsonSerializeWith jsonSerializerOptions data
            }
            |> Request.sendTAsync
            >>= Response.deserializeJsonTAsync<'R>(CancellationToken.None)

        member this.getRequest<'R>(cfg: ApiConfig) =
            http {
                GET cfg.Endpoint
                AuthorizationBearer cfg.ApiKey
                Accept "application/json"
                CacheControl "no-cache"
            }
            |> Request.sendTAsync
            >>= Response.deserializeJsonTAsync<'R>(CancellationToken.None)
