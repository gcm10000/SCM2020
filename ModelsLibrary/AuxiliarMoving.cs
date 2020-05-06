using System;

namespace ModelsLibrary
{
    public class ProductFromJson<T>
    {
        public T Product { get; set; }
    }
    public class AuxiliarConsumption : ProductBase 
    {
        public double Quantity { get; set; }
    }
    public class AuxiliarPermanent : ProductBase { }
    public abstract class ProductBase
    {
        public int Id { get; set; }
        /// <summary>
        /// Produtos retirado na movimentação.
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Data de quando foi realizada saída do material.
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Matrícula do funcionário do Sistema de Controle de Materiais.
        /// </summary>
        public string SCMRegistration { get; set; }
    }
}
