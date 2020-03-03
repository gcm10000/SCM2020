using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCM2020___Server.Context;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCM2020___Server.Models
{
    /// <summary>
    /// Informações sobre o produto. Esta classe contém números sobre o produto.
    /// </summary>
    public class ConsumptionProduct
    {
        public ConsumptionProduct(string raw)
        {
            var productFromRaw = JObject.Parse(raw);
            this.Block = productFromRaw.Value<string>("Block");
            this.Code = productFromRaw.Value<int>("Code");
            this.Description = productFromRaw.Value<string>("Description");
            this.Drawer = productFromRaw.Value<uint>("Drawer");
            this.Id = productFromRaw.Value<int>("Id");
            this.Localization = (Localization)productFromRaw.Value<int>("Localization");
            this.MaximumStock = productFromRaw.Value<double>("MaximumStock");
            this.MininumStock = productFromRaw.Value<double>("MininumStock");
            this.Photo = productFromRaw.Value<string>("Photo");
            this.Stock = productFromRaw.Value<double>("Stock");
            this.Unity = productFromRaw.Value<string>("Unity");

            //this.Group = group.Id;
            //this.Vendor = vendor.Id;
            this.Group = productFromRaw.Value<int>("Group");
            this.Vendor = productFromRaw.Value<int>("Vendor");
        }
        public ConsumptionProduct()
        {

        }
        /// <summary>
        /// Chave primária.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Código do produto.
        /// </summary>
        [Required]
        public int Code { get; set; }
        /// <summary>
        /// Grupo que o produto se encontra.
        /// </summary>
        [ForeignKey("GroupId")]
        public int? Group { get; set; }
        /// <summary>
        /// Nome e descrição do produto.
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// Imagem do produto.
        /// É recomendável que neste campo esteja apenas uma URL referenciando a imagem.
        /// </summary>
        public string Photo { get; set; }
        /// <summary>
        /// Bloco onde produto se encontra.
        /// </summary>
        public string Block { get; set; }
        /// <summary>
        /// Onde o produto se encontra dentro do Sistema de Materiais.
        /// Se no bloco há gaveta, prateleira, colunas ou armários dos quais poderão estar o produto.
        /// </summary>
        public Localization Localization { get; set; }
        /// <summary>
        /// Número da gaveta que se encontra o produto. Isso se estiver na gaveta.
        /// </summary>
        public uint Drawer { get; set; }
        /// <summary>
        /// Fornecedor do produto.
        /// </summary>
        [ForeignKey("VendorId")]
        public int? Vendor { get; set; }
        /// <summary>
        /// Quantidade atual do produto encontrado no estoque.
        /// </summary>
        [Required]
        public double Stock { get; set; }
        /// <summary>
        /// Quantidade mínima cujo produto necessita.
        /// </summary>
        [Required]
        public double MininumStock { get; set; }
        /// <summary>
        /// Quantidade máxima cujo produto necessita.        
        /// </summary>
        [Required]
        public double MaximumStock { get; set; }
        /// <summary>
        /// Unidade quantitativa utilizada para medida.
        /// </summary>
        [Required]
        public string Unity { get; set; }
    }
    public enum Localization
    {
        /// <summary>
        /// Gaveta
        /// </summary>
        Drawer = 0,
        /// <summary>
        /// Prateleira
        /// </summary>
        Shelf = 1,
        /// <summary>
        /// Coluna
        /// </summary>
        Column = 2,
        /// <summary>
        /// Armário
        /// </summary>
        Wardrobe = 3
    }
}
