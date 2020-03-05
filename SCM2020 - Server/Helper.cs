using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace SCM2020___Server
{
    public static class Helper
    {
        public static async Task<string> RawFromBody(ControllerBase controllerBase)
        {
            string postData = string.Empty;
            var stream = controllerBase.Request.Body;
            using (var sr = new StreamReader(stream))
            {
                postData = await sr.ReadToEndAsync();
            }
            return postData;
        }

    }
}
