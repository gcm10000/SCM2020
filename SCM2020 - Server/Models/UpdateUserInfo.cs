using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SCM2020___Server.Models
{
    public class UpdateUserInfo
    {
        [Required]
        public string Registration { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public bool IsPJERJRegistration { get; set; }

    }
}
