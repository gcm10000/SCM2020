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
        public string ThirdParty { get; set; }
        public Business Business { get; set; }
        public Sector Sector { get; set; }
        public PositionInSector? Position { get; set; }
        public string Photo { get; set; }
        public string NameBusiness { get; set; }
        public string NameSector { get; set; }
        public InfoUser() { }
        public InfoUser(string Id, string Name, string Register, string ThirdParty, Business Business, Sector Sector, PositionInSector? Position, string Photo)
        {
            this.Id = Id;
            this.Name = Name;
            this.ThirdParty = ThirdParty;
            this.Business = Business;
            this.Register = Register;
            this.Sector = Sector;
            this.Position = Position;
            this.Photo = Photo;
        }
    }
}
