using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class StoreMessage
    {
        public INotification Notification { get; }
        public string[] UsersId { get; }
        public StoreMessage(INotification notification, string[] UsersId)
        {
            Notification = notification;
            this.UsersId = UsersId;
        }

    }
}
