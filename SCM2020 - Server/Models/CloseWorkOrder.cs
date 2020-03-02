using System;
using System.ComponentModel.DataAnnotations;

namespace SCM2020___Server.Models
{
    public class WorkOrder
    {
        /// <summary>
        /// Ordem de serviço.
        /// </summary>
        [Required]
        public string Work_Order { get; set; }
        /// <summary>
        /// Data da movimentação.
        /// </summary>
        [Required]
        public DateTime MovingDate { get; set; }

    }
}
