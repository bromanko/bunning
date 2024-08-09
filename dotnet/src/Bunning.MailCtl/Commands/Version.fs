namespace Bunning.MailCtl.Commands

open System.Reflection

module Version =
    let exec () =
        let assembly = Assembly.GetExecutingAssembly()

        let versionAttribute =
            assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()

        versionAttribute.InformationalVersion
