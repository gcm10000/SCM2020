using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebAssemblyLibrary
{
    class SocketHub : Hub
    {
        public async Task SendMessage(string window, object data)
        {
            await Clients.All.SendAsync("ReceiveMessage", window, data);
        }
    }
}
