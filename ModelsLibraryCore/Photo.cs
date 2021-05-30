using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelsLibraryCore
{
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// É recomendável que neste campo esteja apenas uma URL referenciando a imagem.
        /// </summary>
        public string Path { get; set; }
    }
}
