using System;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibraryCore
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
