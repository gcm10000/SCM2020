using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    public class MaterialInputByVendor
    {
        public MaterialInputByVendor() { }
        public MaterialInputByVendor(string raw, string UserId)
        {
            var input = JsonConvert.DeserializeObject<MaterialInputByVendor>(raw);
            this.Invoice = input.Invoice;
            this.MovingDate = input.MovingDate;
            this.SCMEmployeeId = UserId;
        }
        /// <summary>
        /// Chave primária da entrada.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Nota fiscal referente a entrada por fornecedor.
        /// </summary>
        public string Invoice { get; set; }
        /// <summary>
        /// Data de movimentação da entrada.
        /// </summary>
        public DateTime MovingDate { get; set; }
        /// <summary>
        /// Produtos de entrada.
        /// Entrada por Id do produto.
        /// </summary>
        public ICollection<ConsumptionProduct> AuxiliarConsumptions { get; set; }
        //Colocar as informações do produto permanente
        //dentro da entrada por fornecedor?
        //public ICollection<AuxiliarPermanent> AuxiliarPermanents { get; set; }
        public int VendorId { get; set; }
        /// <summary>
        /// Id do funcionário que cadastrou a entrada.
        /// </summary>
        public string SCMEmployeeId { get; set; }
    }
}
