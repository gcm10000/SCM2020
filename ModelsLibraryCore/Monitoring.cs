using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibraryCore
{
    public class Monitoring : WorkOrder
    {
        public Monitoring() { }
        public Monitoring(string raw, string SCMEmployeeId)
        {
            Monitoring monitoringRaw = JsonConvert.DeserializeObject<Monitoring>(raw);
            this.EmployeeId = monitoringRaw.EmployeeId;
            this.SCMEmployeeId = SCMEmployeeId;
            this.MovingDate = monitoringRaw.MovingDate;
            this.ClosingDate = monitoringRaw.ClosingDate;
            this.Situation = false;
            this.Work_Order = monitoringRaw.Work_Order;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Data do fechamento da ordem de serviço.
        /// </summary>
        public DateTime ClosingDate { get; set; }
        /// <summary>
        /// Funcionário que gerou o monitoramento da movimentação.
        /// </summary>
        [Required]
        public string SCMEmployeeId { get; set; }
        /// <summary>
        /// Funcionário que solicitou o monitoramento da movimentação.
        /// </summary>
        [Required]
        public string EmployeeId { get; set; }
        /// <summary>
        /// Situação da ordem de serviço.
        /// False = Em aberto.
        /// True = Fechada.
        /// </summary>
        public bool Situation { get; set; }
    }
}
