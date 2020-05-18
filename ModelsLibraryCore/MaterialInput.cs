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
            this.DocDate = input.DocDate;
            //this.EmployeeId = input.EmployeeId;
            this.MovingDate = input.MovingDate;
            this.ConsumptionProducts = input.ConsumptionProducts;
            this.Regarding = input.Regarding;
            //this.SCMEmployeeId = UserId;
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
        /// Funcionário que solicitou a entrada do material.
        /// </summary>
        //[Required]
        //public string EmployeeId { get; set; }
        ///// <summary>
        ///// Funcionário que fez a entrada do material.
        ///// </summary>
        //[Required]
        //public string SCMEmployeeId { get; set; }
        /// <summary>
        /// Data da movimentação.
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
        /// Materiais consumíveis da devolução.
        /// </summary>
        [Required]
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
