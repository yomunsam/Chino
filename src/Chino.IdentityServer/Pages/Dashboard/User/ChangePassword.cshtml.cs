using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.ViewModels.Dashboard.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Chino.IdentityServer.Pages.Dashboard.User
{
    /// <summary>
    /// 【后台面板】的改用户密码功能
    /// </summary>
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ChinoUser> m_UserManager;

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string TargetUserId { get; set; }

        public ChinoUser TargetUser { get; set; }

        [BindProperty]
        public AdminChangePasswordInputModel InputModel { get; set; }

        public ChangePasswordModel(UserManager<ChinoUser> userManager)
        {
            this.m_UserManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            TargetUser = await m_UserManager.FindByIdAsync(TargetUserId);
            if(TargetUser == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await m_UserManager.FindByIdAsync(TargetUserId);
            if (user == null)
                return NotFound();

            var result = await m_UserManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
                result = await m_UserManager.AddPasswordAsync(user, InputModel.NewPassword);
                if (result.Succeeded)
                {
                    return LocalRedirect(ReturnUrl ?? "~/");
                }
            }

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        default:
                            ModelState.AddModelError(string.Empty, error.Description);
                            break;
                    }
                }
            }

            return Page();
        }
    }
}
