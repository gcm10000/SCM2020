using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibraryCore
{
    /// <summary>
    /// Esta classe serve para uma visão individual do produto permanente.
    /// </summary>
    public class PermanentProduct
    {
        public PermanentProduct(string raw)
        {
            var newproduct = JsonConvert.DeserializeObject<PermanentProduct>(raw);
            this.InformationProduct = newproduct.InformationProduct;
            this.Patrimony = newproduct.Patrimony;
            this.Status = newproduct.Status;
            this.DateAdd = newproduct.DateAdd;
            this.WorkOrder = newproduct.WorkOrder;
        }
        public PermanentProduct()
        {

        }
        /// <summary>
        /// Chave primária.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Id do produto como produto consumível.
        /// </summary>
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
        /// <summary>
        /// Indica em qual ordem de serviço o produto se encontra.
        /// </summary>
        public string WorkOrder { get; set; }
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
