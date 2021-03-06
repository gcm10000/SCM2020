﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibraryCore
{
    public class Sector
    {
        public Sector() { }
        public Sector(string raw)
        {
            var sector = JsonConvert.DeserializeObject<Sector>(raw);
            this.NameSector = sector.NameSector;
            this.NumberSectors = sector.NumberSectors;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public List<NumberSectors> NumberSectors { get; set; }
        [Required]
        public string NameSector { get; set; }
    }
}
