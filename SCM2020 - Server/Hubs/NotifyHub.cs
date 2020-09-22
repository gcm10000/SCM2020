using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Hubs
{
    public class NotifyHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            //Clients
            return base.OnConnectedAsync();
        }
    }
}
