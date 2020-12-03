using System.ComponentModel.DataAnnotations;
using Chino.IdentityServer.DataValidation;
using Chino.IdentityServer.DataValidation.Attributes;

namespace Chino.IdentityServer.Dtos.Account
{
    public class RegisterPageDto
    {
        //[Required(ErrorMessage = "username_required")]
        [RegisterRequired(RegisterRequiredType.UserName)]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "email_required")]
        [EmailAddress(ErrorMessage = "invalid_email_address")]
        [RegisterRequired(RegisterRequiredType.Email)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegisterRequired(RegisterRequiredType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "passwd_required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "confirm_password_required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "invalid_confirm_password")]
        public string ConfirmPassword { get; set; }
        
    }
}
