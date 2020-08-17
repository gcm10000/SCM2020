using Newtonsoft.Json;
using System;

namespace ModelsLibrary
{
    public class Monitoring : WorkOrder
    {
        public Monitoring() { }
        public Monitoring(string raw)
        {
            Monitoring monitoringRaw = JsonConvert.DeserializeObject<Monitoring>(raw);
            this.EmployeeId = monitoringRaw.EmployeeId;
            this.SCMEmployeeId = monitoringRaw.SCMEmployeeId;
            this.MovingDate = monitoringRaw.MovingDate;
            this.ClosingDate = monitoringRaw.ClosingDate;
            this.Situation = false;
            this.Work_Order = monitoringRaw.Work_Order;
            this.ServiceLocation = monitoringRaw.ServiceLocation;
        }
        public int Id { get; set; }
        /// <summary>
        /// Data do fechamento da ordem de serviço.
        /// </summary>
        public DateTime? ClosingDate { get; set; }
        /// <summary>
        /// Funcionário que gerou a movimentação.
        /// </summary>
        public string SCMEmployeeId { get; set; }
        /// <summary>
        /// Funcionário que solicitou a movimentação.
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// Situação da ordem de serviço.
        /// False = Em aberto.
        /// True = Fechada.
        /// </summary>
        public bool Situation { get; set; }
        /// <summary>
        /// Setor do qual solicitou a movimentação de saída.
        /// Será tratado como ID da tabela Sectors.
        /// </summary>
        public int RequestingSector { get; set; }
        /// <summary>
        /// Local onde será utilizado o produto.
        /// </summary>
        public string ServiceLocation { get; set; }
    }
}
