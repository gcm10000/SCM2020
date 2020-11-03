using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Position() { }
        public Position(string Name) { this.Name = Name; }
    }
}
