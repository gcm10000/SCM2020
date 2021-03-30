using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SCM2020___Client.Models
{
    public class Notification
    {
        public string Message { get; }
        public System.Drawing.Icon DrawingIcon { get; }
        public MaterialDesignThemes.Wpf.Icon MaterialDesignIcon { get; }
        public DateTime Date { get; }
        public int Code { get; }

        public Notification(string message, System.Drawing.Icon icon, DateTime date, int code)
        {
            this.Message = message;
            this.Date = date;
            this.Code = code;
            this.DrawingIcon = icon;
        }
        public Notification(string message, MaterialDesignThemes.Wpf.Icon icon, DateTime date, int code)
        {
            this.Message = message;
            this.Date = date;
            this.Code = code;
            this.MaterialDesignIcon = icon;
        }
    }
}
