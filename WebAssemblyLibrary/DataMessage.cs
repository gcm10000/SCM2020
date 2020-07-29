using System;
using System.Collections.Generic;
using System.Text;

namespace WebAssemblyLibrary
{
    public class DataMessage
    {

        public string Window { get; }
        public object Data { get; }

        public DataMessage(string window, object data)
        {
            Window = window;
            Data = data;
        }

    }
}
