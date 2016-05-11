namespace FSharp.AspNetCore.ServiceWrapper

open System
open System.Collections.Generic
open System.Linq
open System.ServiceProcess
open System.Text
open System.Configuration
open System.Configuration.Install

module Program = 

    let getInstaller() =
        let installer = new AssemblyInstaller(typedefof<ServiceWrapper>.Assembly, null);
        installer.UseNewContext <- true
        installer

    let installService() =
        let installer = getInstaller()
        let dic = new System.Collections.Hashtable()
        installer.Install(dic)
        installer.Commit(dic)

    let uninstallService() =
        let installer = getInstaller()
        let dic = new System.Collections.Hashtable()
        installer.Uninstall(dic)


    [<EntryPoint>]
    let Main(args) = 
        match (args |> Seq.length) with
        |1 -> match (args.[0]) with
              | "-install" -> installService()
              | "-uninstall" -> uninstallService()
              | "/console" -> 
                    Console.WriteLine("Server starting...");
                    let service = new ServiceWrapper()
                    service.Start
                    Console.WriteLine("Server started. Press <Enter> to Stop.")
                    Console.ReadLine() |> ignore
                    service.Stop
              |_-> failwith (sprintf "Unrecognized param %s" args.[0])
        |_ -> ServiceBase.Run [| new ServiceWrapper() :> ServiceBase |]

        // main entry point return
        0