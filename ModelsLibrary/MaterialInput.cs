using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ModelsLibrary
{
    public class MaterialInput
    {
        public MaterialInput() { }
        public MaterialInput(string raw)
        {
            var input = JsonConvert.DeserializeObject<MaterialInput>(raw);
            this.MovingDate = input.MovingDate;
            this.ConsumptionProducts = input.ConsumptionProducts;
            this.Regarding = input.Regarding;
            this.WorkOrder = input.WorkOrder;
        }
        /// <summary>
        /// Chave primária referente ao registro da devolução de material.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Referência da movimentação.
        /// </summary>
        public Regarding Regarding { get; set; }
        /// <summary>
        /// Data da movimentação.
        /// </summary>
        public DateTime MovingDate { get; set; }
        /// <summary>
        /// Ordem de serviço.
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// Materiais da devolução.
        /// </summary>
        public ICollection<AuxiliarConsumption> ConsumptionProducts { get; set; }
        /// <summary>
        /// Materiais permanentes da devolução.
        /// </summary>
        public ICollection<AuxiliarPermanent> PermanentProducts { get; set; }
    }
    public enum Regarding
    {
        InternalTransfer = 0,
        NotUsed = 1,
        AnotherCounty = 2,
    }
}
