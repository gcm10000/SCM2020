using Newtonsoft.Json;
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
            var output = JsonConvert.DeserializeObject<MaterialOutput>(raw);

            var productFromRaw = JObject.Parse(raw);
            this.MovingDate = productFromRaw.Value<DateTime>("MovingDate");
            this.WorkOrder = productFromRaw.Value<string>("WorkOrder");
            this.ServiceLocation = productFromRaw.Value<string>("ServiceLocation");
            this.WorkOrder = productFromRaw.Value<string>("WorkOrder");

            this.ConsumptionProducts = output.ConsumptionProducts;
            this.PermanentProducts = output.PermanentProducts;
            
            //this.ConsumptionProducts = ((JArray)productFromRaw["ConsumptionProducts"]).ToObject<List<AuxiliarConsumption>>();
            //this.PermanentProducts = ((JArray)productFromRaw["PermanentProducts"]).ToObject<List<AuxiliarPermanent>>();
            //this.ConsumptionProducts = new List<AuxiliarConsumption>();
            //this.PermanentProducts = new List<AuxiliarPermanent>();
            //arrayConsumpterProducts.ForEach(x => this.ConsumptionProducts.Add(new AuxiliarConsumption() { ProductId = x.,  Date = x.Product.Date }));
            //arrayPermanentProducts.ForEach(x => this.PermanentProducts.Add(new AuxiliarPermanent() { ProductId = x.Product.ProductId, Date = x.Product.Date }));
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
        ///// <summary>
        ///// Funcionário do Sistema de Controle de Materiais.
        ///// </summary>
        //public string SCMEmployeeId { get; set; }
        ///// <summary>
        ///// Funcionário que solicitou a movimentação de saída.
        ///// </summary>
        //public string EmployeeId { get; set; }
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
