using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration Configuration;
        private delegate void MessageStartedHandler(List<StoreMessage> storeMessages, string ConnectionId, User user);
        public static ConnectionsRepository Connections { get; } = new ConnectionsRepository();
        public NotifyHub(ControlDbContext controlDbContext, IConfiguration Configuration)
        {
            this.context = controlDbContext;
            this.Configuration = Configuration;
        }

        public override Task OnConnectedAsync()
        {
            var user = JsonConvert.DeserializeObject<User>(Context.GetHttpContext().Request.Query["user"]);
            Connections.Add(Context.ConnectionId, user);
            var messages = context.StoreMessage.Include(x => x.UsersId).Include(x => x.Notification).Where(x => x.UsersId.Any(y => y.UserId == user.Id));

            //MessageStartedHandler d = new MessageStartedHandler(MessageToSend);
            //d.Invoke(new List<StoreMessage>(messages), Context.ConnectionId, user);
            if (messages.Count() > 0)
                return base.OnConnectedAsync();

            MessageToSend(new List<StoreMessage>(messages), Context.ConnectionId, user);

            //Task.Run(SaveChanges);

            return base.OnConnectedAsync();
        }
        private async void MessageToSend(List<StoreMessage> storeMessages, string ConnectionId, User user)
        {
            var options = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<ControlDbContext>();
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            using (var context = new Context.ControlDbContext(options.Options))
            {
                foreach (var message in storeMessages)
                {
                    SendNotify(ConnectionId, message.Notification.ToJson());
                    context.UsersId.Remove(message.UsersId.Single(x => x.UserId == user.Id));
                    if (message.UsersId.Count > 0)
                        context.StoreMessage.Update(message);
                    else
                        context.StoreMessage.Remove(message);
                }
                await context.SaveChangesAsync();
            }
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
