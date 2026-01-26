using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Authentications
{
    public class ResetPasswordByEmailDto
    {
        [Required(ErrorMessage = "Email Is Required")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                   ErrorMessage = "Email Must Be In A Valid Format Like example@domain.com")]
        public string Email { get; set; }
    }
}
