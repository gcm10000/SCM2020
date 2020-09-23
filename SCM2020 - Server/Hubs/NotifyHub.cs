using Microsoft.AspNetCore.SignalR;
using ModelsLibraryCore;
using Newtonsoft.Json;
using SCM2020___Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Hubs
{
    public class NotifyHub : Hub
    {
        private readonly static ConnectionsRepository connections = new ConnectionsRepository();
        public override Task OnConnectedAsync()
        {
            var user = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Request.Query["user"]);
            connections.Add(Context.ConnectionId, user);
            Clients.All.SendAsync("notify", connections.GetAllUser(), user);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            connections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Message message)
        {
            await Clients.Client(connections.GetUserId(message.Destination)).SendAsync("Receive", message.Sender, message.Data);
        }

        public async void SendNotify()
        {
            foreach (var user in connections.GetAllUser())
            {
                Clients.All.SendAsync("notify", connections.GetAllUser(), user);
            }
        }
        public async void Send(string uniqueID, string message)
        {
            await Clients.Client(uniqueID).SendAsync("notify", message);
        }
    }
}
