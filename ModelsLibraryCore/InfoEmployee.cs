using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelsLibraryCore
{
    public class InfoEmployee
    {
        /// <summary>
        /// Matrícula do funcionário a ser cadastrado.
        /// </summary>
        [Required]
        public string Register { get; set; }
        /// <summary>
        /// Nome do funcionário.
        /// </summary>
        [Required(ErrorMessage = "Insira o nome.")]
        public string Name { get; set; }
        /// <summary>
        /// Setor do funcionário.
        /// </summary>
        [Required(ErrorMessage = "Insira o setor.")]
        public int Sector { get; set; }
        public int? Business { get; set; }
        public PositionInSector? Position { get; set; }
    }
}
