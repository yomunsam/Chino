using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chino.IdentityServer.Dtos.Account
{
    public class RegisterPageDto
    {
        [Required(ErrorMessage = "username_required")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "invalid_email_address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "passwd_required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "confirm_password_required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "invalid_confirm_password")]
        public string ConfirmPassword { get; set; }

    }
}
