using Microsoft.AspNetCore.SignalR;
using ModelsLibraryCore;
using Newtonsoft.Json;
using SCM2020___Server.Context;
using SCM2020___Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Hubs
{
    public class NotifyHub : Hub
    {
        private ControlDbContext context;
        public static ConnectionsRepository Connections { get; } = new ConnectionsRepository();
        public NotifyHub(ControlDbContext ControlDbContext)
        {
            this.context = ControlDbContext;
        }

        public override Task OnConnectedAsync()
        {
            var user = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Request.Query["user"]);
            Connections.Add(Context.ConnectionId, user);
            var messages = context.StoreMessage.Where(x => x.UsersId.Contains(user.Id));
            foreach (var message in messages)
            {
                SendNotify(Context.ConnectionId, message.Notification.Message);
                List<string> users = new List<string>(message.UsersId);
                users.Remove(Context.ConnectionId);
                message.UsersId = users.ToArray();
                context.StoreMessage.Update(message);
            }

            //SendToAll($"{user.Id} está conectado.");
            Task.Run(SaveChanges);

            return base.OnConnectedAsync();
        }
        public async void SaveChanges()
        {
            await context.SaveChangesAsync();

        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Connections.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        //public async Task SendMessage(Message message)
        //{
        //    await Clients.Client(connections.GetUserId(message.Destination)).SendAsync("Receive", message.Sender, message.Data);
        //}


        public async void SendToAll(string message)
        {
            await Clients.All.SendAsync("notify", message);
        }

        public async void SendNotify(string uniqueID, string message)
        {
            //if (connections)
            await Clients.Client(uniqueID).SendAsync("notify", message);
        }
    }
}
