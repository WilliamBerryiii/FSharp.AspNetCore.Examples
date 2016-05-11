namespace FSharp.AspNetCore.Examples

open System
open System.IO
open System.Text
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.StaticFiles

open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Configuration.Json
open Microsoft.Extensions.Configuration.EnvironmentVariables
open Microsoft.Extensions.Logging

open SignalR

type Startup() = 

    member __.ConfigureServices(services: IServiceCollection) =

        services.AddOptions() |> ignore
        services.AddLogging() |> ignore
        services.AddMvcCore().AddJsonFormatters() |> ignore

        // Set Up SignalR
        services.AddSignalR(fun options -> 
                options.Hubs.EnableDetailedErrors <- true        
            ) |> ignore
        ()
        
    member __.Configure (app : IApplicationBuilder) = 
  
        let defaultFilesOptions = new DefaultFilesOptions()
        defaultFilesOptions.DefaultFileNames.Clear()
        defaultFilesOptions.DefaultFileNames.Add("index.html")


        app.UseDefaultFiles(defaultFilesOptions) |> ignore
        app.UseSignalR<SignalRConnection>("/signalrConn") |> ignore
        app.UseMvc()|> ignore
        app.Run(fun ctx -> sprintf "HelloWorld" |> ctx.Response.WriteAsync)
            
module SiteRunner = 

    let configuration = (new ConfigurationBuilder())
                            .AddInMemoryCollection(dict["server.urls","http://localhost:8090";])
                            .AddEnvironmentVariables()
                            .Build()

    let start = async { 
                try
                    WebHostBuilder()
                        .UseKestrel()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseConfiguration(configuration)
                        .UseStartup<Startup>()
                        .Build()
                        .Run()
                with 
                | :? System.AggregateException as ae -> printfn "%s" (ae.Flatten().Message)
                }