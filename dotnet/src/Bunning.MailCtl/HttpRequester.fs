namespace Bunning.MailCtl

open System.Net
open System.Net.Http
open System.Text.Json
open System.Text.Json.Serialization
open System.Threading
open System.Threading.Tasks
open FsHttp
open FsToolkit.ErrorHandling
open FsToolkit.ErrorHandling.Operator.Task

type ApiConfig = { Endpoint: string; ApiKey: string }

type IHttpRequester =
    abstract member postRequest<'Req, 'Err, 'Res>: cfg: ApiConfig -> data: 'Req -> Task<Result<'Res, 'Err>>
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

    member this.DeserializeResult<'T, 'E>(response: Response): Task<Result<'T, 'E>> =
        match response.statusCode with
        | HttpStatusCode.OK ->
            response
            |> Response.deserializeJsonTAsync<'T>(CancellationToken.None)
            |> Task.map Ok
        | _ ->
            response
            |> Response.deserializeJsonTAsync<'E>(CancellationToken.None)
            |> Task.map Error

    interface IHttpRequester with
        member this.postRequest<'Req, 'Err, 'Res>(cfg: ApiConfig) (data: 'Req) =
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
            >>= this.DeserializeResult<'Res, 'Err>

        member this.getRequest<'R>(cfg: ApiConfig) =
            http {
                GET cfg.Endpoint
                AuthorizationBearer cfg.ApiKey
                Accept "application/json"
                CacheControl "no-cache"
            }
            |> Request.sendTAsync
            >>= Response.deserializeJsonTAsync<'R>(CancellationToken.None)
