﻿using ModelsLibraryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Repositories
{
    //https://medium.com/tableless/criando-um-chat-completo-com-asp-net-core-mvc-e-signalr-ec0380f28dbe
    public class ConnectionsRepository
    {
        private readonly Dictionary<string, User> connections = new Dictionary<string, User>();
        public void Add(string uniqueID, User user)
        {
            if (!connections.ContainsKey(uniqueID))
            {
                connections.Add(uniqueID, user);
            }
        }
        public string GetUserId(long id)
        {
            return connections.FirstOrDefault(x => x.Value.Key == id).Key;
        }
        public List<User> GetAllUser()
        {
            return (from con in connections
                    select con.Value
            ).ToList();
        }
    }
}