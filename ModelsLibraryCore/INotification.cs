using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Windows.Forms;

namespace ModelsLibraryCore
{
    public interface INotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public StoreMessage StoreMessage { get; set; }
        public ToolTipIcon Icon { get; set; }
        public string Message { get; set; }
        public ICollection<Destination> Destination { get; set; }

    }
}
