using System;
using System.IO;
using System.IO.Packaging;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace SCM2020___Client
{
    static class Helper
    {
        public static Uri Server = new Uri("http://192.168.1.30:52991/api/");
        public static AuthenticationHeaderValue Authentication { get; set; } = null;
        private static readonly Regex NumberRegex = new Regex("[^0-9,]+"); //regex that matches disallowed text
        public static string TemplatePath = @"C:\Users\Gabriel\source\repos\gcm10000\SCM2020\SCM2020 - Client\Templates\"; //File.ReadAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "movement.cshtml"));
        //public static string SCMRegistration { get; set; }
        public static string NameIdentifier { get; set; }
        public static string Role { get; set; }
        public static ModelsLibraryCore.Sector CurrentSector { get; set; }
        public static bool IsTextAllowed(string text)
        {
            return !NumberRegex.IsMatch(text);
        }
        public static string GetTempFilePathWithExtension(string extension)
        {
            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + extension;
            return Path.Combine(path, fileName);
        }

        /// <summary>
        ///  Funciona apenas após a página estiver totalmente carregada.
        /// </summary>
        public static bool PrintDocument(WebBrowser webBrowser)
        {
            MSHTML.IHTMLDocument2 doc = webBrowser.Document as MSHTML.IHTMLDocument2;
            return doc.execCommand("Print", true, null);
        }
    }
}
