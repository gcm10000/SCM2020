using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibraryCore
{
    public class MaterialInput
    {
        public MaterialInput() { }
        public MaterialInput(string raw)
        {
            var input = JsonConvert.DeserializeObject<MaterialInput>(raw);
            this.MovingDate = input.MovingDate;
            this.ConsumptionProducts = input.ConsumptionProducts;
            this.PermanentProducts = input.PermanentProducts;
            this.Regarding = input.Regarding;
            this.WorkOrder = input.WorkOrder;
        }
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
        /// Data da movimentação.
        /// </summary>
        [Required]
        public DateTime MovingDate { get; set; }
        /// <summary>
        /// Ordem de serviço.
        /// </summary>
        [Required]
        public string WorkOrder { get; set; }
        /// <summary>
        /// Materiais consumíveis da devolução.
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
