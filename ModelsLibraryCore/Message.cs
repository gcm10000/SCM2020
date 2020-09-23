using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class Message
    {
        public Int64 Destination { get; set; }
        public User Sender { get; set; }
        public string Data { get; set; }
    }
}
