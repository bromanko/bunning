namespace Bunning.MailCtl

open MailKit
open MailKit.Search
open MailKit.Net.Imap
open MailKit.Security
open FSharp.Control
open FsToolkit.ErrorHandling

module Imap =
    [<RequireQualifiedAccess>]
    type T() =
        let mutable client = new ImapClient()

        member this.Connect host port (userName: string) password =
            task {
                if client.IsConnected then
                    return Ok()
                else
                    try
                        do! client.ConnectAsync(host, port, SecureSocketOptions.Auto)
                        do! client.AuthenticateAsync(userName, password)
                        return Ok()
                    with ex ->
                        return Error ex
            }

        member this.Disconnect() =
            task {
                if client.IsConnected = false then
                    return Ok()
                else
                    try
                        do! client.DisconnectAsync(quit = true)
                        client.Dispose()
                        return Ok()
                    with ex ->
                        return Error ex
            }


        member this.GetMessageIds() =
            task {
                try
                    do! client.Inbox.OpenAsync(FolderAccess.ReadOnly) |> Task.ignore
                    let! ids = client.Inbox.SearchAsync(SearchQuery.All)
                    return ids |> Ok
                with ex ->
                    return Error ex
            }

        member this.GetMessage(msgId: UniqueId) =
            task {
                try
                    do! client.Inbox.OpenAsync(FolderAccess.ReadOnly) |> Task.ignore
                    let! msg = client.Inbox.GetMessageAsync(msgId)
                    return Ok msg
                with ex ->
                    return Error ex
            }

        interface System.IDisposable with
            member this.Dispose() = client.Dispose()
