using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCM2020___Server.Models
{
    public class MaterialOutput
    {
        /// <summary>
        /// Chave primária referente ao registro do material.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Data de quando a movimentação de saída foi requisitada do Sistema de Controle de Materiais.
        /// </summary>
        public DateTime MovingDate { get; set; }
        /// <summary>
        /// Matrícula do funcionário do Sistema de Controle de Materiais.
        /// </summary>
        public string SCMRegistration { get; set; }
        /// <summary>
        /// Matrícula do funcionário que solicitou a movimentação de saída.
        /// </summary>
        public string EmployeeRegistration { get; set; }
        /// <summary>
        /// Setor do qual solicitou a movimentação de saída.
        /// Será tratado como ID da tabela Sectors.
        /// </summary>
        public int RequestingSector { get; set; }
        /// <summary>
        /// Ordem de serviço.
        /// </summary>
        public string WorkOrder { get; set; }
        /// <summary>
        /// Local onde será utilizado o produto.
        /// </summary>
        public string ServiceLocation { get; set; }
        /// <summary>
        /// Produtos retirados na movimentação de saída.
        /// </summary>
        public PermanentProduct[] Products { get; set; }

    }
}
