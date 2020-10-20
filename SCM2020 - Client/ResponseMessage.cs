using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ModelsLibraryCore.RequestingClient
{
    public class ResponseMessage
    {
        public ResponseMessage(string Result, HttpResponseMessage HttpResponseMessage)
        {
            this.Result = Result;
            this.HttpResponseMessage = HttpResponseMessage;
        }
        public string Result { get; }
        public HttpResponseMessage HttpResponseMessage { get; }
    }
}
