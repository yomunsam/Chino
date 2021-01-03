using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Services;
using Chino.IdentityServer.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.User
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly CommonLocalizationService L;

        [BindProperty]
        public ChangePasswordInputModel InputModel { get; set; }

        public ChangePasswordModel(UserManager<ChinoUser> userManager,
            CommonLocalizationService commonLocalizationService)
        {
            this.m_UserManager = userManager;
            this.L = commonLocalizationService;
        }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var user = await m_UserManager.GetUserAsync(this.User);
            var result = await m_UserManager.ChangePasswordAsync(user, InputModel.CurrentPassword, InputModel.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToPage("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "PasswordMismatch":
                            ModelState.AddModelError(string.Empty, L["Err_PasswordMismatch"]);
                            break;
                        default:
                            ModelState.AddModelError(string.Empty, error.Description);
                            break;
                    }
                }
                return Page();
            }
        }
    }
}
