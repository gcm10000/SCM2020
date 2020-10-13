using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ModelsLibraryCore
{
    class SolicitationMessage : INotification
    {
        public ToolTipIcon Icon { get; }
        public string Message { get; }
    }
}
