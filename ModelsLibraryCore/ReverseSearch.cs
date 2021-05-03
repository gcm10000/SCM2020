using System;
using System.Collections.Generic;
using System.Text;

namespace ModelsLibraryCore
{

    public enum Movement
    {
        Output,
        Input,
        Devolution
    }
    public enum Type
    {
        Consumpter,
        Permanent
    }

    public class ReverseSearch
    {

        public double Stock { get; set; }
        public string Unity { get; set; }
        public string WorkOrder { get; set; }
        public string Invoice { get; set; }
        public Movement Movement { get; set; }
        public string ServiceLocalization { get; set; }
        /// <summary>
        /// Data do fechamento da ordem de serviço.
        /// </summary>
        public DateTime? ClosingDate { get; set; }
        /// <summary>
        /// Data da movimentação.
        /// </summary>
        public DateTime MovingDate { get; set; }
        public Type Type { get; set; }
        public string Patrimony { get; set; }
    }
}
