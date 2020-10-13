using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ModelsLibraryCore
{
    public class NotificationMessage
    {
        public ToolTipIcon Icon { get; }
        public int Code { get; }
        public string Description { get; }

        public string Message { get; }

        public NotificationMessage(ToolTipIcon icon, int code, string description, string message)
        {
            Icon = icon;
            Code = code;
            Description = description;
            Message = message;
        }
    }
}
