using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SCM2020___Server
{
    public class ImageInput
    {
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public IFormFile Image { get; set; }
    }
}
