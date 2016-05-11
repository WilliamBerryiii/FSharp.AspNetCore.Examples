namespace FSharp.AspNetCore.ServiceWrapper

open System
open System.IO
open System.Text
open System.Diagnostics
open System.ServiceProcess
open System.Threading
open log4net
open System.Threading.Tasks

open FSharp.AspNetCore.Examples

type public ServiceWrapper() as service = 
    inherit ServiceBase()

    let eventLog = new EventLog()

    let cancellationTokenSource = new CancellationTokenSource()

    member this.Start = 
        service.ServiceName <- "Accelerator.MessageStoreBuilder"

        let eventSource = "Accelerator.MessageStoreBuildere"
        if not (EventLog.SourceExists(eventSource)) then
            EventLog.CreateEventSource(eventSource, "Service");

        eventLog.Source <- eventSource;
        eventLog.Log <- "Service";

        log4net.Config.XmlConfigurator.Configure() |> ignore

        let websiteTask = Async.Start(SiteRunner.start, cancellationToken = cancellationTokenSource.Token)
        
        ()

    member this.Stop = 
        ()

    // TODO define your service operations
    override service.OnStart(args:string[]) =
        base.OnStart(args)
        eventLog.WriteEntry("Service Starting")
        service.Start
        eventLog.WriteEntry("Service Started")

    override service.OnStop() =
        base.OnStop()
        eventLog.WriteEntry("Service Stopping")
        service.Stop
        eventLog.WriteEntry("Service Stopped")