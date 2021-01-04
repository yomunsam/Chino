using System.ComponentModel.DataAnnotations;

namespace Chino.IdentityServer.ViewModels.Dashboard.User
{
    public class AdminChangePasswordInputModel
    {
        [Required(ErrorMessage = "new_password_required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "invalid_new_confirm_password")]
        public string ConfirmNewPassword { get; set; }
    }
}
