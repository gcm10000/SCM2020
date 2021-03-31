using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SCM2020___Client.Models
{
    public class Notification
    {
        public string Message { get; }
        private System.Drawing.Icon DrawingIcon { get; }
        public ImageSource DrawingImage { get; }
        public MaterialDesignThemes.Wpf.Icon MaterialDesignIcon { get; }
        public string RelativeTime { get; set; }
        public int Code { get; }
        public DateTime DateTime { get; }
        public bool IsNewest { get; set; } = true;

        public Notification(string message, System.Drawing.Icon icon, DateTime date, int code)
        {
            this.Message = message;
            this.DateTime = date;
            this.RelativeTime = Helper.RelativeTime(date);
            this.Code = code;
            this.DrawingIcon = icon;
            this.DrawingImage = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
        public Notification(string message, MaterialDesignThemes.Wpf.Icon icon, DateTime date, int code)
        {
            this.Message = message;
            this.DateTime = date;
            this.RelativeTime = Helper.RelativeTime(date);
            this.Code = code;
            this.MaterialDesignIcon = icon;
        }
    }
}
