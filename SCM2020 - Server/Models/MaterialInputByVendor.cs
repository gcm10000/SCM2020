using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCM2020___Server.Models
{
    public class MaterialInputByVendor
    {
        /// <summary>
        /// Chave primária da entrada.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Nota fiscal referente a entrada por fornecedor.
        /// </summary>
        public string Invoice { get; set; }
        /// <summary>
        /// Data de movimentação da entrada.
        /// </summary>
        public DateTime MovingDate { get; set; }
        /// <summary>
        /// Produtos de entrada.
        /// </summary>
        public PermanentProduct[] Products { get; set; }

    }
}
