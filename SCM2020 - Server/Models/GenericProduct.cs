using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCM2020___Server.Models
{

    /// <summary>
    /// Esta classe serve para uma visão geral dos produtos de um mesmo código.
    /// </summary>
    public abstract class GenericProduct
    {
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
        [ForeignKey("GroupID")]
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
        [ForeignKey("VendorID")]
        public int? Vendor { get; set; }
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
