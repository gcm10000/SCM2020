using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SCM2020___Client
{
    static class Helper
    {
        public static Uri Server = new Uri("http://gabriel-laptop:52991/api/");
        private static readonly Regex NumberRegex = new Regex("[^0-9,]+"); //regex that matches disallowed text
        public static bool IsTextAllowed(string text)
        {
            return !NumberRegex.IsMatch(text);
        }
    }
}
