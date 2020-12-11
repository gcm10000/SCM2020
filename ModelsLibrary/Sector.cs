
using System.Collections.Generic;

namespace ModelsLibrary
{
    public class Sector
    {
        public Sector(string raw)
        {
            
        }
        public Sector() { }
        public int Id { get; set; }
        public List<NumberSectors> NumberSectors { get; set; }
        public string NameSector { get; set; }
    }
}
