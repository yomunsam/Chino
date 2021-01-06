using System.ComponentModel.DataAnnotations;

namespace Chino.IdentityServer.ViewModels.Account
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "passwd_required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }
}
