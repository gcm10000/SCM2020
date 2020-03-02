using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCM2020___Server.Models
{
    public class MaterialInput
    {
        /// <summary>
        /// Chave primária referente ao registro da devolução de material.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Referência da movimentação.
        /// </summary>
        public Regarding Regarding { get; set; }
        /// <summary>
        /// Funcionário que solicitou a entrada do material.
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }
        /// <summary>
        /// Funcionário que fez a entrada do material.
        /// </summary>
        [Required]
        public int SCMEmployeeId { get; set; }
        /// <summary>
        /// Data da movimentação do material.
        /// </summary>
        [Required]
        public DateTime MovingDate { get; set; }
        /// <summary>
        /// Data da Ordem de Serviço.
        /// </summary>
        public DateTime DocDate { get; set; }
        /// <summary>
        /// Ordem de serviço.
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// Materiais de entrada.
        /// </summary>
        [Required]
        public PermanentProduct[] Products { get; set; }
    }
    public enum Regarding
    {
        InternalTransfer = 0,
        NotUsed = 1,
        AnotherCounty = 2,

    }
}
