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
        [JsonIgnore]
        public MaterialInputByVendor MaterialInputByVendor { get; set; }
    }
    public class AuxiliarPermanent : ProductBase { }
    public abstract class ProductBase
    {
        [JsonIgnore]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Produtos retirado na movimentação.
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// Data de quando foi realizada a movimentação.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }
        [JsonIgnore]
        public MaterialOutput MaterialOutput { get; set; }
    }
}
