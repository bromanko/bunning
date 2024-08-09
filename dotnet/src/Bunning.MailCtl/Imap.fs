namespace Bunning.MailCtl

open MailKit
open MailKit.Search
open MailKit.Net.Imap
open MailKit.Security
open FSharp.Control

module Imap =
    let getMessageIds host port (userName: string) (password: string) =
        task {
            use client = new ImapClient()

            try
                do! client.ConnectAsync(host, port, SecureSocketOptions.Auto)
                do! client.AuthenticateAsync(userName, password)
                let! _ = client.Inbox.OpenAsync(FolderAccess.ReadOnly)
                let! ids = client.Inbox.SearchAsync(SearchQuery.All)
                return ids |> Result.Ok
            with ex ->
                return Result.Error ex
        }
