namespace Bunning.MailCtl

open System.Net.Http
open System.Text.Json
open System.Text.Json.Serialization
open System.Threading
open System.Threading.Tasks
open FsHttp
open FsToolkit.ErrorHandling.Operator.Task

type ApiConfig = { Endpoint: string; ApiKey: string }

type IHttpRequester =
    abstract member postRequest<'T, 'R>: cfg: ApiConfig -> data: 'T -> Task<'R>
    abstract member getRequest<'R>: cfg: ApiConfig -> Task<'R>

type HttpRequester() =
    let jsonSerializerOptions () =
        let jOpts = JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower)
        JsonFSharpOptions.Default().AddToJsonSerializerOptions(jOpts)
        jOpts

    let logBody (req: HttpRequestMessage) =
        let s = req.Content.ReadAsStringAsync() |> Async.AwaitTask |> Async.RunSynchronously
        printfn $"%A{s}"
        req

    interface IHttpRequester with
        member this.postRequest<'T, 'R>(cfg: ApiConfig) (data: 'T) =
            http {
                POST cfg.Endpoint
                AuthorizationBearer cfg.ApiKey
                Accept "application/json"
                CacheControl "no-cache"
                body
                ContentType "application/json"
                jsonSerializeWith (jsonSerializerOptions()) data
                config_transformHttpRequestMessage logBody
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
