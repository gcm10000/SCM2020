using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Windows.Forms;

namespace ModelsLibraryCore
{
    public class SolicitationMessage : INotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [JsonIgnore]
        public StoreMessage StoreMessage { get; set; }
        
        public ICollection<Destination> Destination { get; set; }
        public ToolTipIcon Icon { get; set; }
        public string Message { get; set; }
        public Monitoring Monitoring { get; set; }
        public SolicitationMessage() { }
        public SolicitationMessage(ToolTipIcon icon, string message, ICollection<Destination> destination, Monitoring monitoring)
        {
            Icon = icon;
            Message = message;
            Destination = destination;
            Monitoring = monitoring;
        }
    }
}
