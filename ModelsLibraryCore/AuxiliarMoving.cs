using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ModelsLibraryCore
{
    public class ProductFromJson<T>
    {
        public T Product { get; set; }

    }
    public class AuxiliarConsumption : ProductBase
    {
        public double Quantity { get; set; }
        /// <summary>
        /// JsonIgnore.
        /// </summary>
        [JsonIgnore]
        public MaterialInputByVendor MaterialInputByVendor { get; set; }
        [NotMapped]
        public string WorkOrder { get; set; }
    }
    public class AuxiliarPermanent : ProductBase { }
    public class AuxiliarPermanentInputByVendor : ProductBase 
    {
        [NotMapped]
        [JsonIgnore]
        public string Patrimony { get; set; }
        [JsonIgnore]
        public MaterialInputByVendor MaterialInputByVendor { get; set; }

    }
    public abstract class ProductBase
    {
        /// <summary>
        /// JsonIgnore.
        /// </summary>
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Produto retirado na movimentação.  
        /// Referencia objeto da classe de produto permanente ou consumível.
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// Data de quando foi realizada a movimentação.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }
        /// <summary>
        /// JsonIgnore.
        /// </summary>
        [JsonIgnore]
        public MaterialOutput MaterialOutput { get; set; }
        /// <summary>
        /// JsonIgnore.
        /// </summary>
        [JsonIgnore]
        public MaterialInput MaterialInput { get; set; }
        /// <summary>
        /// Id do funcionário do Sistema de Controle de Materiais.
        /// </summary>
        public string SCMEmployeeId { get; set; }
    }
}
