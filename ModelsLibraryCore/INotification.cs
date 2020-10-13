using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ModelsLibraryCore
{
    public interface INotification
    {
        public ToolTipIcon Icon { get; }
        public string Message { get; }
        public int[] Destination { get; }

    }
}
