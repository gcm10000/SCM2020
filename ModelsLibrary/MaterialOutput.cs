﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ModelsLibrary
{
    public class MaterialOutput
    {
        public MaterialOutput(string raw)
        {
            var productFromRaw = JObject.Parse(raw);
            this.MovingDate = productFromRaw.Value<DateTime>("MovingDate");
            this.WorkOrder = productFromRaw.Value<string>("WorkOrder");
            //this.EmployeeId = productFromRaw.Value<string>("EmployeeId");
            //this.SCMEmployeeId = UserId;

            this.ConsumptionProducts = ((JArray)productFromRaw["ConsumptionProducts"]).ToObject<List<AuxiliarConsumption>>();
            this.PermanentProducts = ((JArray)productFromRaw["PermanentProducts"]).ToObject<List<AuxiliarPermanent>>();
            //this.ConsumptionProducts = new List<AuxiliarConsumption>();
            //this.PermanentProducts = new List<AuxiliarPermanent>();
            //arrayConsumpterProducts.ForEach(x => this.ConsumptionProducts.Add(new AuxiliarConsumption() { ProductId = x.,  Date = x.Product.Date }));
            //arrayPermanentProducts.ForEach(x => this.PermanentProducts.Add(new AuxiliarPermanent() { ProductId = x.Product.ProductId, Date = x.Product.Date }));
        }
        public MaterialOutput()
        {

        }
        /// <summary>
        /// Chave primária referente ao registro do material.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Data de quando a movimentação de saída foi requisitada do Sistema de Controle de Materiais.
        /// </summary>
        public DateTime MovingDate { get; set; }
        ///// <summary>
        ///// Funcionário do Sistema de Controle de Materiais.
        ///// </summary>
        //public string SCMEmployeeId { get; set; }
        ///// <summary>
        ///// Funcionário que solicitou a movimentação de saída.
        ///// </summary>
        //public string EmployeeId { get; set; }
        /// <summary>
        /// Ordem de serviço.
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// Somente produtos de consumo retirados na movimentação de saída.
        /// </summary>
        public ICollection<AuxiliarConsumption> ConsumptionProducts { get; set; }
        /// <summary>
        /// Somente produtos permanentes retirados na movimentação de saída.
        /// </summary>
        public ICollection<AuxiliarPermanent> PermanentProducts { get; set; }

    }
}
