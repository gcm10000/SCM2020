using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;

namespace SCM2020___Utility
{
    /// <summary>
    /// Informações sobre o produto. Esta classe contém números sobre o produto.
    /// </summary>
    public class ConsumptionProduct
    {
        public ConsumptionProduct(string raw)
        {
            var productFromRaw = JObject.Parse(raw);
            this.Code = productFromRaw.Value<int>("Code");
            this.Description = productFromRaw.Value<string>("Description");
            this.NumberLocalization = productFromRaw.Value<uint>("NumberLocalization");
            this.Id = productFromRaw.Value<int>("Id");
            this.Localization = productFromRaw.Value<string>("Localization");
            this.MaximumStock = productFromRaw.Value<double>("MaximumStock");
            this.MininumStock = productFromRaw.Value<double>("MininumStock");
            this.Photo = productFromRaw.Value<string>("Photo");
            this.Stock = productFromRaw.Value<double>("Stock");
            this.Unity = productFromRaw.Value<string>("Unity");

            this.Group = productFromRaw.Value<int>("Group");
        }
        public ConsumptionProduct() { }
        public HttpStatusCode AddProduct(string url, ConsumptionProduct consumptionProduct, AuthenticationHeaderValue Authorization)
        {
            using (var client = new HttpClient())
            {
                //Limpa o header
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = Authorization;
                //incluir o cabeçalho Accept que será envia na requisição             
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Envio da requisição a fim de autenticar
                // e obter o token de acesso
                HttpResponseMessage respToken = client.PostAsync(url, new StringContent(
                        JsonConvert.SerializeObject(consumptionProduct), Encoding.UTF8, "application/json")).Result;

                //obtem o resultado
                string content = respToken.Content.ReadAsStringAsync().Result;

                if (respToken.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new AuthenticationException(content);
                }
                return respToken.StatusCode;
            }

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
        /// Onde o produto se encontra dentro do Sistema de Materiais.
        /// </summary>
        public string Localization { get; set; }
        /// <summary>
        /// Número da matriz referente ao bloco e linha.
        /// </summary>
        public uint NumberLocalization { get; set; }
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
