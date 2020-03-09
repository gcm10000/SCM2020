using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibraryCore
{
    public class Sector
    {
        public Sector(string raw)
        {
            
        }
        public Sector() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int NumberSector { get; set; }
        [Required]
        public string NameSector { get; set; }
    }
}
