﻿using Newtonsoft.Json.Linq;
using System;

namespace ModelsLibrary
{
    /// <summary>
    /// Esta classe serve para uma visão individual do produto permanente.
    /// </summary>
    public class PermanentProduct
    {
        public PermanentProduct(string raw)
        {
            var newproduct = JObject.Parse(raw);
            this.InformationProduct = newproduct.Value<int>("InformationProduct");
            this.Patrimony = newproduct.Value<string>("Patrimony");
            this.Status = newproduct.Value<Status>("Status");
            this.DateAdd = DateTime.Now;
        }
        public PermanentProduct()
        {

        }
        /// <summary>
        /// Chave primária.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id do produto como produto genérico.
        /// </summary>
        public int InformationProduct { get; set; }
        /// <summary>
        /// Como se encontra o produto.
        /// </summary>
        public Status Status { get; set; }
        /// <summary>
        /// Data de quando o produto foi cadastrado na base de dados.
        /// </summary>
        public DateTime DateAdd { get; set; }
        /// <summary>
        /// Número de patrimônio.
        /// </summary>
        public string Patrimony { get; set; }
        /// <summary>
        /// Indica em qual ordem de serviço o produto se encontra.
        /// </summary>
        public string WorkOrder { get; set; }
    }
    public enum Status
    {
        /// <summary>
        /// Novo
        /// </summary>
        New = 0,
        /// <summary>
        /// Recondicionado
        /// </summary>
        Recondicioned = 1,
        /// <summary>
        /// Descartado
        /// </summary>
        Discarded = 2
    }
}
