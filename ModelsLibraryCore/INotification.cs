using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ModelsLibraryCore
{
    public interface INotification
    {
        public ToolTipIcon Icon { get; }
        public int Code { get; }
        public string Message { get; }

    }
}
