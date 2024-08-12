namespace Bunning.MailCtl

open System
open PuppeteerSharp
open FsToolkit.ErrorHandling

module HtmlRenderer =
    let defaultExecutablePath () = "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome"

    [<RequireQualifiedAccess>]
    type T(executablePath: string) =
        let mutable browser: IBrowser = null

        member this.Render (html: string) = task {
            if browser = null then
                let opts = LaunchOptions(ExecutablePath = executablePath, HeadlessMode = HeadlessMode.False)
                let! b = Puppeteer.LaunchAsync(opts)
                browser <- b

            let! page = browser.NewPageAsync()
            do! page.SetContentAsync(html)
            do! page.GetContentAsync() |> Task.ignore
            let opts = ScreenshotOptions(FullPage = true)
            do! page.ScreenshotAsync($"/Users/bromanko/Desktop/test${Guid.NewGuid().ToString()}.png", opts)
            return Ok()
        }

        member IDisposable.Dispose() =
            if browser <> null then
                browser.Dispose()