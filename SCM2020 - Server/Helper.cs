﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLibraryCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SCM2020___Server
{
    public static class Helper
    {
        public static List<ApplicationUser> Users { get; set; }
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
        public static string Combine(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
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
        public static bool MultiplesContains(this string bigstr, params string[] content)
        {
            bigstr = bigstr.ToLowerInvariant();
            content = content.Select(s => s.ToLowerInvariant()).ToArray();
                foreach (var item in content)
                {
                    if (!content.Any(x => bigstr.Contains(item.RemoveDiacritics())))
                    {
                        continue;
                    }
                    return true;
                }
                return false;
        }
        public static bool MultiplesContainsWords(this string mystr, params string[] words)
        {
            mystr = mystr.ToLowerInvariant().RemoveDiacritics();
            words = words.Select(s => s.ToLowerInvariant().RemoveDiacritics()).ToArray();
            
            bool contains = true;

            foreach (var word in words)
            {
                contains = contains && mystr.Contains(word);
            }
            return contains;
        }
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
       public static bool IsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
        public static string ToJson<T>(this T obj)
        {
            string output = JsonConvert.SerializeObject(obj);
            return output;
        }
        public static int NextAvaliable(this IEnumerable<int> myInts)
        {
            int firstAvailable = Enumerable.Range(1, Int32.MaxValue).Except(myInts).First();
            return firstAvailable;
        }

        public static ImageFormat GetImageFormat(byte[] bytes)
        {
            // see http://www.mikekunz.com/image_file_header.html  
            var bmp = Encoding.ASCII.GetBytes("BM");     // BMP
            var gif = Encoding.ASCII.GetBytes("GIF");    // GIF
            var png = new byte[] { 137, 80, 78, 71 };    // PNG
            var tiff = new byte[] { 73, 73, 42 };         // TIFF
            var tiff2 = new byte[] { 77, 77, 42 };         // TIFF
            var jpeg = new byte[] { 255, 216, 255, 224 }; // jpeg
            var jpeg2 = new byte[] { 255, 216, 255, 225 }; // jpeg canon

            if (bmp.SequenceEqual(bytes.Take(bmp.Length)))
                return ImageFormat.bmp;

            if (gif.SequenceEqual(bytes.Take(gif.Length)))
                return ImageFormat.gif;

            if (png.SequenceEqual(bytes.Take(png.Length)))
                return ImageFormat.png;

            if (tiff.SequenceEqual(bytes.Take(tiff.Length)))
                return ImageFormat.tiff;

            if (tiff2.SequenceEqual(bytes.Take(tiff2.Length)))
                return ImageFormat.tiff;

            if (jpeg.SequenceEqual(bytes.Take(jpeg.Length)))
                return ImageFormat.jpeg;

            if (jpeg2.SequenceEqual(bytes.Take(jpeg2.Length)))
                return ImageFormat.jpeg;

            return ImageFormat.unknown;
        }
    }
}
