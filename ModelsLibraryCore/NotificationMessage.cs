using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ModelsLibraryCore
{
    public class ProductMessage : INotification
    {
        public string Description { get; }
        public ToolTipIcon Icon { get; }
        public string Message { get; }

        public ProductMessage(ToolTipIcon icon, int code, string description, string message)
        {
            Icon = icon;
            Code = code;
            Description = description;
            Message = message;
        }
    }
}
