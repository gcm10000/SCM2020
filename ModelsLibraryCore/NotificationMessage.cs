using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace ModelsLibraryCore
{
    public class NotificationMessage
    {
        public System.Drawing.Icon Icon { get; }
        public int Code { get; }
        public string Description { get; }

        public string Message { get; }

        public NotificationMessage(Icon icon, int code, string description, string message)
        {
            Icon = icon;
            Code = code;
            Description = description;
            Message = message;
        }
    }
}
