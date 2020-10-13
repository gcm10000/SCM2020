using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ModelsLibraryCore
{
    class AlertStockMessage : INotification
    {
        public ToolTipIcon Icon { get; }
        public string Message { get; }
        public int[] Destination { get; }
        public int Code { get; }
        public string Description { get; }

        public AlertStockMessage(ToolTipIcon icon, string message, int[] destination, int code, string description)
        {
            Icon = icon;
            Message = message;
            Destination = destination;
            Code = code;
            Description = description;
        }
    }
}
