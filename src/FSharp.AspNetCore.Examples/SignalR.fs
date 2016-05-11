namespace FSharp.AspNetCore.Examples

open Microsoft.AspNetCore.SignalR
open Microsoft.AspNetCore.SignalR.Hubs

open System.Threading.Tasks

module SignalR = 
    
    type SignalRConnection() as this = 
        inherit PersistentConnection()
        override x.OnConnected(req,id) =
            this.Connection.Send(id, "Hello World") |> ignore
            base.OnConnected(req,id)
        override x.OnReceived(req, id, data) = 
            this.Connection.Send(id, "Message Received: " + data) |> ignore
            base.OnReceived(req, id, data)
      
    type IHubClient = 
        abstract member Notify : string -> unit
            
    [<HubName("SampleHub")>]
    type SampleHub() as this = 
        inherit Hub<IHubClient>()
        override x.OnConnected() =
            base.OnConnected()
        member x.Message(text : string) : unit = 
            this.Clients.All.Notify(text)
            