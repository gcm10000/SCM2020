using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCM2020___Server.Models
{
    public class ConsumptionOutput
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Produtos de consumo retirados na movimentação de saída.
        /// </summary>
        [Required]
        public int ConsumperId { get; set; }
    }
    public class PermanentOutput
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Produtos permanentes retirados na movimentação de saída.
        /// </summary>
        [Required]
        public int PermanentId { get; set; }
    }
}
