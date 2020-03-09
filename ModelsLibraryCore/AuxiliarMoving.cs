using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    public class ProductFromJson<T>
    {
        public T Product { get; set; }
    }
    public class AuxiliarConsumption : ProductBase { }
    public class AuxiliarPermanent : ProductBase { }
    public abstract class ProductBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Produtos retirado na movimentação.
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// Data de quando foi realizada saída do material.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }
    }
}
