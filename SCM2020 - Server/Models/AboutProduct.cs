using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCM2020___Server.Context;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SCM2020___Server.Models
{
    /// <summary>
    /// Informações sobre o produto. Esta classe contém números sobre o produto.
    /// </summary>
    public class AboutProduct : GenericProduct
    {
        public AboutProduct(string raw)
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
        public AboutProduct()
        {

        }
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
}
