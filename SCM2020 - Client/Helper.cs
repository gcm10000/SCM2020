using System;
using System.IO;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace SCM2020___Client
{
    static class Helper
    {
        public static Uri Server = new Uri("http://gabriel-laptop:52991/api/");
        public static AuthenticationHeaderValue Authentication { get; set; } = null;
        private static readonly Regex NumberRegex = new Regex("[^0-9,]+"); //regex that matches disallowed text
        public static string TemplatePath = @"C:\Users\Gabriel\source\repos\gcm10000\SCM2020\SCM2020 - Client\Templates\"; //File.ReadAllText(System.IO.Path.Combine(Environment.CurrentDirectory, "movement.cshtml"));
        public static string SCMRegistration { get; set; }
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
    }
}
