using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
        public static JwtSecurityToken GetToken(ControllerBase controllerBase)
        {
            bool b = controllerBase.Request.Headers.ContainsKey("Authorization");
            var header = controllerBase.Request.Headers["Authorization"].ToString();
            var bearer = "Bearer ";
            var accessToken = header.Substring(bearer.Length);
            var handler = new JwtSecurityTokenHandler();
            var o = handler.ReadToken(accessToken) as JwtSecurityToken;
            return o;
        }
        public static Int32 Levenshtein(String a, String b)
        {

            if (string.IsNullOrEmpty(a))
            {
                if (!string.IsNullOrEmpty(b))
                {
                    return b.Length;
                }
                return 0;
            }

            if (string.IsNullOrEmpty(b))
            {
                if (!string.IsNullOrEmpty(a))
                {
                    return a.Length;
                }
                return 0;
            }

            Int32 cost;
            Int32[,] d = new int[a.Length + 1, b.Length + 1];
            Int32 min1;
            Int32 min2;
            Int32 min3;

            for (Int32 i = 0; i <= d.GetUpperBound(0); i += 1)
            {
                d[i, 0] = i;
            }

            for (Int32 i = 0; i <= d.GetUpperBound(1); i += 1)
            {
                d[0, i] = i;
            }

            for (Int32 i = 1; i <= d.GetUpperBound(0); i += 1)
            {
                for (Int32 j = 1; j <= d.GetUpperBound(1); j += 1)
                {
                    cost = Convert.ToInt32(!(a[i - 1] == b[j - 1]));

                    min1 = d[i - 1, j] + 1;
                    min2 = d[i, j - 1] + 1;
                    min3 = d[i - 1, j - 1] + cost;
                    d[i, j] = Math.Min(Math.Min(min1, min2), min3);
                }
            }

            return d[d.GetUpperBound(0), d.GetUpperBound(1)];
        }
    }
}
