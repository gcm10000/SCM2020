using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class Result
    {
        public int? Id { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public Result(int? Id, string Message, object Data)
        {
            this.Id = Id;
            this.Message = Message;
            this.Data = Data;
        }
    }
}
