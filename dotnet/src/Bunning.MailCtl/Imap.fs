namespace Bunning.MailCtl

open MailKit
open MailKit.Search
open MailKit.Net.Imap
open MailKit.Security
open FSharp.Control
open FsToolkit.ErrorHandling

module Imap =
    let connect host port (userName: string) password =
        task {
            let client = new ImapClient()

            try
                do! client.ConnectAsync(host, port, SecureSocketOptions.Auto)
                do! client.AuthenticateAsync(userName, password)
                return Ok client
            with ex ->
                return Error ex

        }

    let disconnect (client: ImapClient) =
        task {
            try
                do! client.DisconnectAsync(quit = true)
                client.Dispose()
                return Ok()
            with ex ->
                return Error ex
        }


    let getMessageIds (client: ImapClient) =
        task {
            try
                do! client.Inbox.OpenAsync(FolderAccess.ReadOnly) |> Task.ignore
                let! ids = client.Inbox.SearchAsync(SearchQuery.All)
                return ids |> Ok
            with ex ->
                return Error ex
        }

    let getMessage (client: ImapClient) (msgId: UniqueId) =
        task {
            try
                do! client.Inbox.OpenAsync(FolderAccess.ReadOnly) |> Task.ignore
                let! msg = client.Inbox.GetMessageAsync(msgId)
                return Ok msg
            with ex ->
                return Error ex
        }
