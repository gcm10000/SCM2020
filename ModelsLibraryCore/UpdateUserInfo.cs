using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
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
