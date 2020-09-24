using ModelsLibraryCore;
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
        public void Remove(string uniqueID)
        {
            if (connections.ContainsKey(uniqueID))
            {
                connections.Remove(uniqueID);
            }
        }
        public bool ExistsUniqueID(string UniqueID)
        {
            return connections.TryGetValue(UniqueID, out _);
        }
        public string GetUserId(long id)
        {
            return connections.FirstOrDefault(x => x.Value.Key == id).Key;
        }
        public string GetKey(User user)
        {
            return connections.FirstOrDefault(x => x.Value == user).Key;
        }
        public List<User> GetAllUser()
        {
            return (from con in connections
                    select con.Value
            ).ToList();
        }
        
    }
}
