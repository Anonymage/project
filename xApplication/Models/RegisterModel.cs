using System.ComponentModel.DataAnnotations;

namespace xApplication.Models
{
    public class RegisterModel : LoginModel
    {
        [EmailAddress(ErrorMessage = "Invalid E-Mail-Adress")]
        [StringLength(254, ErrorMessage = "Mail cannot exceed 254 Characters")]
        [MinLength(3, ErrorMessage = "Mail cannot be shorter than 3 Characters")]
        public string MAIL { get; set; }
    }
}
