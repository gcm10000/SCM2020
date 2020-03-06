using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class DeleteUser
    {
        [Required]
        public string UserName { get; set; }
    }
}
