using System.ComponentModel.DataAnnotations;

namespace xApplication.Models
{
    public class SettingsModel : RegisterModel
    {
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "New Password cannot exceed 20 Characters")]
        public string NEWPASSWORD { get; set; }

        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Confirm-Password cannot exceed 20 Characters")]
        public string CONFIRMPASSWORD { get; set; }

        public string IMGURL { get; set; }
    }
}
