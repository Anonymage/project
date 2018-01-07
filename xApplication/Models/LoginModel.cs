using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace xApplication.Models
{
    public class LoginModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(20, ErrorMessage = "Username cannot exceed 20 Characters")]
        [MinLength(4, ErrorMessage = "Username cannot be shorter than 4 Characters")]
        public string USERNAME { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "Password cannot exceed 20 Characters")]
        [MinLength(8, ErrorMessage = "Password cannot be shorter than 8 Characters")]
        public string PASSWORD { get; set; }
    }
}
