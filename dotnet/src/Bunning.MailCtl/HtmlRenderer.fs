namespace Bunning.MailCtl

open System
open PuppeteerSharp

module HtmlRenderer =
    [<RequireQualifiedAccess>]
    type T(executablePath: string) =
        let mutable browser: IBrowser = null

        member this.Render (html: string) = task {
            if browser = null then
                let opts = LaunchOptions(ExecutablePath = executablePath, HeadlessMode = HeadlessMode.False)
                let! b = Puppeteer.LaunchAsync(opts)
                browser <- b

            return Error <| (NotImplementedException("Not implemented") :> exn)
        }

        member IDisposable.Dispose() =
            if browser <> null then
                browser.Dispose()