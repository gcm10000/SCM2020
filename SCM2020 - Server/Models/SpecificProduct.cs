using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCM2020___Server.Models
{
    /// <summary>
    /// Esta classe serve para uma visão individual do produto.
    /// </summary>
    public class SpecificProduct
    {
        public SpecificProduct(string raw)
        {
            var newproduct = JObject.Parse(raw);
            this.InformationProduct = newproduct.Value<int>("InformationProduct");
            this.Patrimony = newproduct.Value<string>("Patrimony");
            this.Status = newproduct.Value<Status>("Status");
            this.DateAdd = DateTime.Now;
        }
        public SpecificProduct()
        {

        }
        /// <summary>
        /// Chave primária.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("AboutProductId")]
        public int InformationProduct { get; set; }
        /// <summary>
        /// Como se encontra o produto.
        /// </summary>
        [Required]
        public Status Status { get; set; }
        /// <summary>
        /// Data de quando o produto foi cadastrado na base de dados.
        /// </summary>
        public DateTime DateAdd { get; set; }
        /// <summary>
        /// Número de patrimônio.
        /// </summary>
        public string Patrimony { get; set; }
    }
    public enum Status
    {
        /// <summary>
        /// Novo
        /// </summary>
        New = 0,
        /// <summary>
        /// Recondicionado
        /// </summary>
        Recondicioned = 1,
        /// <summary>
        /// Descartado
        /// </summary>
        Discarded = 2
    }
}
