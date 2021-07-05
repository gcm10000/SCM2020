using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibraryCore
{
    public class MaterialInputByVendor
    {
        public MaterialInputByVendor() { }
        public MaterialInputByVendor(string raw, string UserId)
        {
            var input = JsonConvert.DeserializeObject<MaterialInputByVendor>(raw);
            this.Invoice = input.Invoice;
            this.VendorId = input.VendorId;
            this.MovingDate = input.MovingDate;
            this.SCMEmployeeId = UserId;
            this.ConsumptionProducts = input.ConsumptionProducts;
            this.PermanentProducts = input.PermanentProducts;
        }
        public MaterialInputByVendor(string raw)
        {
            var input = JsonConvert.DeserializeObject<MaterialInputByVendor>(raw);
            this.Invoice = input.Invoice;
            this.VendorId = input.VendorId;
            this.MovingDate = input.MovingDate;
            this.SCMEmployeeId = input.SCMEmployeeId;
            this.ConsumptionProducts = input.ConsumptionProducts;
            this.PermanentProducts = input.PermanentProducts;
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
        [Required]
        public string Invoice { get; set; }
        /// <summary>
        /// Data de movimentação da entrada.
        /// </summary>
        [Required]
        public DateTime MovingDate { get; set; }
        /// <summary>
        /// Produtos de entrada.
        /// Entrada por Id do produto.
        /// </summary>
        public ICollection<AuxiliarConsumption> ConsumptionProducts { get; set; }
        /// <summary>
        /// Materiais permanentes presente na entrada.
        /// </summary>
        public ICollection<AuxiliarPermanentInputByVendor> PermanentProducts { get; set; }
        [Required]
        public int VendorId { get; set; }
        /// <summary>
        /// Id do funcionário que cadastrou a entrada.
        /// </summary>
        [Required]
        public string SCMEmployeeId { get; set; }
    }
}
