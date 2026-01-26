using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Authentications
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage = "Email Is Required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
           ErrorMessage = "Email Must Be In A Valid Format Like example@domain.com")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Token Is Required")]
        public string Token { get; set; }


        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                   ErrorMessage = "Password Must Be At Least 8 Characters, Contain Uppercase, Lowercase, Number, And Special Character.")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password Does not Match The Password")]
        public string ConfirmPassword { get; set; }
    }
}
