using System.ComponentModel.DataAnnotations;

namespace SCM2020___Server.Models
{
    public class DeleteUser
    {
        [Required]
        public string UserName { get; set; }
    }
}
