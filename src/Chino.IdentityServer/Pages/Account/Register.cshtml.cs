using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Dtos.Account;
using Chino.IdentityServer.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Chino.IdentityServer.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ChinoAccountConfiguration m_AccountConfiguration;
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly SignInManager<ChinoUser> m_SignInManager;
        private readonly IStringLocalizer<RegisterModel> L;

        public RegisterModel(ChinoAccountConfiguration chinoAccountConfiguration,
            UserManager<ChinoUser> userManager,
            SignInManager<ChinoUser> signInManager,
            IStringLocalizer<RegisterModel> localizer)
        {
            m_AccountConfiguration = chinoAccountConfiguration;
            m_UserManager = userManager;
            m_SignInManager = signInManager;
            L = localizer;
        }

        [BindProperty]
        public RegisterPageDto RegisterDto { get; set; }

        [BindProperty]
        public string ReturnUrl { get; set; }

        public IActionResult OnGetAsync(string returnUrl = null)
        {
            if (!m_AccountConfiguration.EnableRegister)
            {
                //TODO: 等做好错误页之后，这里最好是跳转到错误页面
                return Redirect("/");
            }
            this.ReturnUrl = returnUrl ?? Url.Content("~/");

            return Page();
        }



        public async Task<IActionResult> OnPostAsync()
        {
            await Task.Yield();
            if (!ModelState.IsValid) return Page();

            var user = new ChinoUser
            {
                UserName = RegisterDto.UserName,
                Email = RegisterDto.Email
            };

            //if (!m_AccountConfiguration.UserName.Register)
            //    user.UserName = RegisterDto.Email;

            var result = await m_UserManager.CreateAsync(user, RegisterDto.Password);
            if (result.Succeeded)
            {
                await m_SignInManager.SignInAsync(user, false);
                return LocalRedirect(ReturnUrl);
            }

            //不成功
            if(result != null)
            {
                foreach(var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "DuplicateUserName":
                            ModelState.AddModelError(string.Empty, L["Error_DuplicateUserName", RegisterDto.UserName]);
                            break;
                        case "DuplicateEmail":
                            ModelState.AddModelError(string.Empty, L["Error_DuplicateEmail", RegisterDto.Email]);
                            break;
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
