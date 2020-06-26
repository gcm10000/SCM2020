using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{
    public class InfoUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Register { get; set; }
        public int IdSector { get; set; }
        public string Sector { get; set; }
        public InfoUser() { }
        public InfoUser(string Id, string Name, string Register, int IdSector, string Sector) 
        {
            this.Id = Id;
            this.Name = Name;
            this.Register = Register;
            this.IdSector = IdSector;
            this.Sector = Sector;
        }
    }
}
