using System;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Enums.Account;
using Chino.IdentityServer.Extensions.Configurations;
using Chino.IdentityServer.Services;
using Chino.IdentityServer.ViewModels.Account;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Chino.IdentityServer.Pages.Account
{
    /// <summary>
    /// 短信验证码登录
    /// </summary>
    public class SMSLoginModel : PageModel
    {
        /// <summary>
        /// 授权服务器交互服务
        /// </summary>
        private readonly IIdentityServerInteractionService m_IdsInteraction;
        private readonly SignInManager<ChinoUser> m_SignInManager;
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly IEventService m_IdsEvent;
        private readonly IStringLocalizer<SMSLoginModel> L;
        private readonly ChinoAccountConfiguration m_AccountConfiguration;
        private readonly CommonLocalizationService CL;


        [BindProperty]
        public string ReturnUrl { get; set; }

        [BindProperty, TempData]
        public ELoginViewType LoginType { get; set; }

        [BindProperty]
        public SMSLoginViewModel LoginViewModel { get; set; }

        public bool EnableLocalLogin { get; set; } = true;

        public bool AllowRememberLogin { get; set; } = true;

        public SMSLoginModel(IIdentityServerInteractionService identityServerInteractionService,
            SignInManager<ChinoUser> signInManager,
            UserManager<ChinoUser> userManager,
            IEventService idsEvent,
            IStringLocalizer<SMSLoginModel> localizer,
            CommonLocalizationService cl,
            ChinoAccountConfiguration accountConfiguration)
        {
            m_IdsInteraction = identityServerInteractionService;
            m_SignInManager = signInManager;
            m_UserManager = userManager;
            m_IdsEvent = idsEvent;
            L = localizer;
            m_AccountConfiguration = accountConfiguration;
            CL = cl;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl, int? loginType)
        {
            this.ReturnUrl = returnUrl;
            if (this.User.Identity.IsAuthenticated)
            {
                return Redirect(this.ReturnUrl ?? "/");
            }
            if (loginType == null)
                this.LoginType = m_AccountConfiguration.DefaultLoginType;
            else
                this.LoginType = (ELoginViewType)Enum.ToObject(typeof(ELoginViewType), loginType.Value);

            switch (this.LoginType)
            {
                case ELoginViewType.UserNameOrEmail:
                    return RedirectToPage("/Account/Login", new { returnUrl = returnUrl, loginType = loginType });
                case ELoginViewType.PhoneAndPassword:
                    return RedirectToPage("/Account/PhoneLogin", new { returnUrl = returnUrl, loginType = loginType });
            }

            if (!m_AccountConfiguration.CanLoginBySMSVerificationCode())
                throw new Exception(L["error_notEnableLoginType"]);


            var context = await m_IdsInteraction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                AllowRememberLogin = false;
                //IdentityString = context?.LoginHint;
            }

            return Page();
        }
    }
}
