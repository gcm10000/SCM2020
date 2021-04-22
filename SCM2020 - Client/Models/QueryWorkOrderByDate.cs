using System;
using System.Collections.Generic;
using System.Text;

namespace SCM2020___Client.Models
{
    public class QueryWorkOrderByDate
    {
        public string WorkOrder { get; set; }
        public DateTime MovingDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string RegisterApplicant { get; set; }
        public string Applicant { get; set; }
    }
}
