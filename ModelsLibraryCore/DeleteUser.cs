using System.ComponentModel.DataAnnotations;

namespace ModelsLibraryCore
{
    public class DeleteUser
    {
        [Required]
        public string UserName { get; set; }
    }
}
