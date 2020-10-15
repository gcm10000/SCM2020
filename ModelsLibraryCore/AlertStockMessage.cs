using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelsLibraryCore
{
    public class AlertStockMessage : INotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [JsonIgnore]
        public StoreMessage StoreMessage { get; set; }
        public ToolTipIcon Icon { get; set; }
        public string Message { get; set; }
        public ICollection<Destination> Destination { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public AlertStockMessage() { }
        public AlertStockMessage(ToolTipIcon icon, string message, ICollection<Destination> destination, int code, string description)
        {
            Icon = icon;
            Message = message;
            Destination = destination;
            Code = code;
            Description = description;
        }
    }
}
