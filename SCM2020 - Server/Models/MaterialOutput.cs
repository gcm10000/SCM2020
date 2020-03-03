using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace SCM2020___Server.Models
{
    public class MaterialOutput
    {
        public MaterialOutput(string raw)
        {
            var productFromRaw = JObject.Parse(raw);
            this.MovingDate = productFromRaw.Value<DateTime>("MovingDate");
            this.WorkOrder = productFromRaw.Value<string>("WorkOrder");
            this.EmployeeRegistration = productFromRaw.Value<string>("EmployeeRegistration");
            this.SCMRegistration = productFromRaw.Value<string>("SCMRegistration");
            this.RequestingSector = productFromRaw.Value<int>("RequestingSector");
            this.ServiceLocation = productFromRaw.Value<string>("ServiceLocation");
            this.WorkOrder = productFromRaw.Value<string>("WorkOrder");

            var arrayConsumpterProducts = ((JArray)productFromRaw["ConsumptionProducts"]).ToObject<List<int>>();
            var arrayPermanentProducts = ((JArray)productFromRaw["PermanentProducts"]).ToObject<List<int>>();
            this.ConsumptionProducts = new List<ConsumptionOutput>();
            this.PermanentProducts = new List<PermanentOutput>();
            arrayConsumpterProducts.ForEach(x => this.ConsumptionProducts.Add(new ConsumptionOutput() { ConsumperId = x }));
            arrayPermanentProducts.ForEach(x => this.PermanentProducts.Add(new PermanentOutput() { PermanentId = x }));

        }
        public MaterialOutput()
        {

        }
        /// <summary>
        /// Chave primária referente ao registro do material.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Data de quando a movimentação de saída foi requisitada do Sistema de Controle de Materiais.
        /// </summary>
        public DateTime MovingDate { get; set; }
        /// <summary>
        /// Matrícula do funcionário do Sistema de Controle de Materiais.
        /// </summary>
        public string SCMRegistration { get; set; }
        /// <summary>
        /// Matrícula do funcionário que solicitou a movimentação de saída.
        /// </summary>
        public string EmployeeRegistration { get; set; }
        /// <summary>
        /// Setor do qual solicitou a movimentação de saída.
        /// Será tratado como ID da tabela Sectors.
        /// </summary>
        public int RequestingSector { get; set; }
        /// <summary>
        /// Ordem de serviço.
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// Local onde será utilizado o produto.
        /// </summary>
        public string ServiceLocation { get; set; }
        /// <summary>
        /// Somente produtos de consumo retirados na movimentação de saída.
        /// </summary>
        public ICollection<ConsumptionOutput> ConsumptionProducts { get; set; }
        /// <summary>
        /// Somente produtos permanentes retirados na movimentação de saída.
        /// </summary>
        public ICollection<PermanentOutput> PermanentProducts { get; set; }

    }
}
