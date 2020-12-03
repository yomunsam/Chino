using System.ComponentModel.DataAnnotations;

namespace Chino.IdentityServer.Dtos.Account
{
    public class LoginWithUsernamePageDto
    {
        [Required(ErrorMessage = "username_required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "passwd_required")]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }
}
