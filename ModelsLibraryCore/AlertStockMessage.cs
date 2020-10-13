using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class AlertStockMessage : INotification
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
