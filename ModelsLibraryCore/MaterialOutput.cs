using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibraryCore
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

            List<ProductFromJson<AuxiliarConsumption>> arrayConsumpterProducts = ((JArray)productFromRaw["ConsumptionProducts"]).ToObject<List<ProductFromJson<AuxiliarConsumption>>>();
            List<ProductFromJson<AuxiliarPermanent>> arrayPermanentProducts = ((JArray)productFromRaw["PermanentProducts"]).ToObject<List<ProductFromJson<AuxiliarPermanent>>>();
            this.ConsumptionProducts = new List<AuxiliarConsumption>();
            this.PermanentProducts = new List<AuxiliarPermanent>();
            arrayConsumpterProducts.ForEach(x => this.ConsumptionProducts.Add(new AuxiliarConsumption() { ProductId = x.Product.ProductId,  Date = x.Product.Date }));
            arrayPermanentProducts.ForEach(x => this.PermanentProducts.Add(new AuxiliarPermanent() { ProductId = x.Product.ProductId, Date = x.Product.Date }));
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
        public ICollection<AuxiliarConsumption> ConsumptionProducts { get; set; }
        /// <summary>
        /// Somente produtos permanentes retirados na movimentação de saída.
        /// </summary>
        public ICollection<AuxiliarPermanent> PermanentProducts { get; set; }

    }
}
