using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.User
{
    /// <summary>
    /// 管理当前登录用户的外部登录信息
    /// </summary>
    public class ExternalLoginsModel : PageModel
    {
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly SignInManager<ChinoUser> m_SignInManager;

        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationScheme> OtherLogins { get; set; }
        public bool ShowRemoveButton { get; set; }

        public ExternalLoginsModel(UserManager<ChinoUser> userManager,
            SignInManager<ChinoUser> signInManager)
        {
            this.m_UserManager = userManager;
            this.m_SignInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await m_UserManager.GetUserAsync(this.User);
            if (user == null)
                return NotFound($"Unable to load user.");

            CurrentLogins = await m_UserManager.GetLoginsAsync(user);
            OtherLogins = (await m_SignInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            ShowRemoveButton = user.PasswordHash != null || CurrentLogins.Count > 1;
            return Page();
        }

        /// <summary>
        /// 连接到新的外部登录服务
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Page("./ExternalLogins", pageHandler: "LinkLoginCallback");
            var properties = m_SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, m_UserManager.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        /// <summary>
        /// 从上面那个方法跳出到外部登录，然后外部登录再跳回来到我们这儿
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
        {
            var user = await m_UserManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{user.Id}'.");
            }

            var info = await m_SignInManager.GetExternalLoginInfoAsync(user.Id);
            if (info == null)
            {
                throw new InvalidOperationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await m_UserManager.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                //StatusMessage = "The external login was not added. External logins can only be associated with one account.";
                return RedirectToPage();
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            //StatusMessage = "The external login was added.";
            return RedirectToPage();
        }
    }
}
