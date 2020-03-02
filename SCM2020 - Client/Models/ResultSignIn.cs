using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace SCM2020___Client.Models
{
    class ResultSignIn
    {
        public ResultSignIn(System.Net.HttpStatusCode StatusCode, Token Token)
        {
            this.StatusCode = StatusCode;
            this.Token = Token;
        }
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public Token Token { get; set; }
    }
}
