using ModelsLibraryCore;
using ModelsLibraryCore.RequestingClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client.Models
{
    public class StockQuery
    {
        public StockQuery(ModelsLibraryCore.ConsumptionProduct consumptionProduct)
        {
            this.Code = consumptionProduct.Code;
            this.Description = consumptionProduct.Description;
            this.NumberLocalization = consumptionProduct.NumberLocalization;
            this.Localization = consumptionProduct.Localization;
            this.MaximumStock = consumptionProduct.MaximumStock;
            this.MininumStock = consumptionProduct.MininumStock;
            this.Photo = consumptionProduct.Photo;
            this.Stock = consumptionProduct.Stock;
            this.Unity = consumptionProduct.Unity;

            this.Group = APIClient.GetData<ModelsLibraryCore.Group>(new Uri(Helper.Server, $"group/{consumptionProduct.Group}").ToString(), Helper.Authentication).GroupName;
            ConsumptionProduct = consumptionProduct;
        }

        /// <summary>
        /// Código do produto.
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Grupo que o produto se encontra.
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// Nome e descrição do produto.
        /// </summary>
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
        public double Stock { get; set; }
        /// <summary>
        /// Quantidade mínima cujo produto necessita.
        /// </summary>
        public double MininumStock { get; set; }
        /// <summary>
        /// Quantidade máxima cujo produto necessita.        
        /// </summary>
        public double MaximumStock { get; set; }
        /// <summary>
        /// Unidade quantitativa utilizada para medida.
        /// </summary>
        public string Unity { get; set; }
        public ConsumptionProduct ConsumptionProduct { get; set; }
    }
}
