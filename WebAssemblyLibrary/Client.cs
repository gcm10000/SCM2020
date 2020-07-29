using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAssemblyLibrary.Client
{
    public class Client
    {
        private HubConnection connection;
        public Client()
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/SocketHub")
                .Build();

            connection.StartAsync().Wait();
        }
        public void Send(string methodName, string window, string message)
        {
            connection.InvokeCoreAsync(methodName, new[] { window, message });
        }
        public void Receive(string methodName, Action Receive)
        {
            connection.On(methodName, Receive);
        }
    }
}
