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
        public MaterialInputByVendor(string raw)
        {
            var input = JsonConvert.DeserializeObject<MaterialInputByVendor>(raw);
            this.Invoice = input.Invoice;
            this.MovingDate = input.MovingDate;
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
        /// </summary>
        
        //Colocar as informações do produto permanente
        //dentro da entrada por fornecedor?
        //public ICollection<ConsumptionProduct> Products { get; set; }
        public int VendorId { get; set; }
        /// <summary>
        /// Id do funcionário que cadastrou a entrada.
        /// </summary>
        public string SCMEmployeeId { get; set; }

    }
}
