﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ModelsLibraryCore
{
    public class SolicitationMessage : INotification
    {
        public ToolTipIcon Icon { get; }
        public string Message { get; }
        public int[] Destination { get; }
        public Monitoring Monitoring { get; }
        //public InputOutput { get; }
        public SolicitationMessage(ToolTipIcon icon, string message, int[] destination, Monitoring monitoring)
        {
            Icon = icon;
            Message = message;
            Destination = destination;
            Monitoring = monitoring;
        }
    }
}
