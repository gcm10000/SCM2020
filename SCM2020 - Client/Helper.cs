using System;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace SCM2020___Client
{
    static class Helper
    {
        public static Uri Server = new Uri("http://gabriel-laptop:52991/api/");
        public static AuthenticationHeaderValue Authentication { get; set; } = null;
        private static readonly Regex NumberRegex = new Regex("[^0-9,]+"); //regex that matches disallowed text
        public static bool IsTextAllowed(string text)
        {
            return !NumberRegex.IsMatch(text);
        }
    }
}
