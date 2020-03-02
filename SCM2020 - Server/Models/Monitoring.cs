using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCM2020___Server.Models
{
    public class Monitoring : WorkOrder
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Data do fechamento da ordem de serviço.
        /// </summary>
        public DateTime ClosingDate { get; set; }
        /// <summary>
        /// Funcionário que gerou a movimentação.
        /// </summary>
        [Required]
        public int SCMEmployeeId { get; set; }
        /// <summary>
        /// Funcionário que solicitou a movimentação.
        /// </summary>
        [Required]
        public int EmployeeId { get; set; }
        /// <summary>
        /// Situação da ordem de serviço.
        /// False = Em aberto.
        /// True = Fechada.
        /// </summary>
        [Required]
        public bool Situation { get; set; }
    }
}
