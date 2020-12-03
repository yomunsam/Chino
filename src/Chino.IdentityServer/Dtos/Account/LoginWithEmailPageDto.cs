using System.ComponentModel.DataAnnotations;

namespace Chino.IdentityServer.Dtos.Account
{
    public class LoginWithEmailPageDto
    {
        [Required(ErrorMessage = "email_required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "passwd_required")]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }
}
