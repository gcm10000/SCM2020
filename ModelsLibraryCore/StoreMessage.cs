using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelsLibraryCore
{
    public class StoreMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int NotificationId { get; set; }
        public INotification Notification { get; }
        public List<UsersId> UsersId { get; set; }
        public StoreMessage() { }
        public StoreMessage(INotification notification, List<UsersId> UsersId)
        {
            Notification = notification;
            this.UsersId = UsersId;
        }

    }
}
