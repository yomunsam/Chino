using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Pages.Account.External
{
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ChinoUser> m_SignInManager;
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly ILogger<ExternalLoginModel> m_Logger;

        public string ReturnUrl { get; set; }
        public string ProviderDisplayName { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public ExternalLoginModel(SignInManager<ChinoUser> signInManager,
            UserManager<ChinoUser> userManager,
            ILogger<ExternalLoginModel> logger)
        {
            this.m_SignInManager = signInManager;
            this.m_UserManager = userManager;
            this.m_Logger = logger;
        }

        public IActionResult OnGet()
        {
            return LocalRedirect("~/");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = m_SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("../Login", new { ReturnUrl = returnUrl });
            }
            var info = await m_SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await m_SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                m_Logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                //if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                //{
                //    Input = new InputModel
                //    {
                //        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                //    };
                //}
                return Page();
            }
        }

    }
}
