using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.UI.ViewModel
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Display(Name ="Email")]
        [Required(ErrorMessage ="Please Enter Your Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(4,ErrorMessage ="Password must be longer than 4 characters")]
        public string Password { get; set; }
    }
}
