using System.ComponentModel.DataAnnotations;

namespace ModelsLibraryCore
{
    public class UpdateUserInfo
    {
        [Required]
        public string Register { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}
