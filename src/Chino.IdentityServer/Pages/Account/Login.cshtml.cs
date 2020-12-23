using System;
using System.Threading.Tasks;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Dtos.Account;
using Chino.IdentityServer.Enums.Account;
using Chino.IdentityServer.Extensions.Configurations;
using Chino.IdentityServer.Extensions.Oidc;
using Chino.IdentityServer.Models.User;
using Chino.IdentityServer.Services;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Nekonya;

namespace Chino.IdentityServer.Pages.Account
{
    public class LoginModel : PageModel
    {
        /// <summary>
        /// 授权服务器交互服务
        /// </summary>
        private readonly IIdentityServerInteractionService m_IdsInteraction;
        private readonly SignInManager<ChinoUser> m_SignInManager;
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly IEventService m_IdsEvent;
        private readonly IStringLocalizer<LoginModel> L;
        private readonly ChinoAccountConfiguration m_AccountConfiguration;
        private readonly CommonLocalizationService CL;

        public LoginModel(IIdentityServerInteractionService identityServerInteractionService,
            SignInManager<ChinoUser> signInManager,
            UserManager<ChinoUser> userManager,
            IEventService idsEvent,
            IStringLocalizer<LoginModel> localizer,
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


        [BindProperty]
        public LoginPageDto LoginDto { get; set; }

        [BindProperty]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 登录的身份验证字段，可能会传入UserName，也有可能是Email，或者别的什么玩意
        /// </summary>
        [BindProperty]
        public string IdentityString { get; set; }

        [BindProperty, TempData]
        public ELoginViewType LoginType { get; set; }

        public bool EnableLocalLogin { get; set; } = true;

        public bool AllowRememberLogin { get; set; } = true;

        public async Task<IActionResult> OnGet(string returnUrl, int? loginType)
        {
            this.ReturnUrl = returnUrl;
            if (loginType == null)
                this.LoginType = m_AccountConfiguration.DefaultLoginType;
            else
                this.LoginType = (ELoginViewType)Enum.ToObject(typeof(ELoginViewType), loginType.Value);

            switch (this.LoginType)
            {
                case ELoginViewType.PhoneAndPassword:
                    return RedirectToPage("/Account/PhoneLogin", new { returnUrl = returnUrl, loginType = loginType });
            }

            var context = await m_IdsInteraction.GetAuthorizationContextAsync(returnUrl);
            if(context?.IdP != null)
            {
                AllowRememberLogin = false;
                IdentityString = context?.LoginHint;
            }


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string button)
        {
            if (this.IdentityString.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, L["xxx_required", m_AccountConfiguration.GetLoginInputText(CL)]);
            }

            var context = await m_IdsInteraction.GetAuthorizationContextAsync(ReturnUrl);
            if(button != "login")
            {
                if(context != null)
                {
                    /*
                     * 如果用户点击取消，则向IdentityServer发送一个结果（类似于用户拒绝同意）】
                     * 然后Oidc错误会被发回给客户端
                     */
                    await m_IdsInteraction.DenyAuthorizationAsync(context, IdentityServer4.Models.AuthorizationError.AccessDenied);

                    if (context.IsNativeClient())
                    {
                        //The client is native, so this change in how to return the response is for better UX for the end user.
                        //客户端是本地客户端，因此此更改返回响应的方式是为最终用户提供更好的UX。
                        //TODO : 这边跳转往后有时间再写
                    }

                    return Redirect("/Index");
                }
                else
                {
                    return Redirect("/Index");
                }
            }

            if (ModelState.IsValid)
            {
                //登录流程
                var user = await this.FindUserByIdentityStringAsync(IdentityString);
                if(user != null)
                {
                    //找到用户了

                    //检查账号是否通过验证
                    if(m_AccountConfiguration.IsNeedToConfirmPhoneNumberWhenRegister())
                    {
                        //注册时候要验证手机号
                        if (!user.PhoneNumberConfirmed)
                        {
                            //没注册，跳转到手机号验证
                            return RedirectToPage("/Account/Confirmation/Phone",new {
                                UserId = user.Id
                            });
                        }
                    }

                    var result = await m_SignInManager.PasswordSignInAsync(user, LoginDto.Password, LoginDto.RememberLogin, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        await m_IdsEvent.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName, clientId: context?.Client.ClientId));

                        if (context != null)
                        {
                            if (context.IsNativeClient())
                            {
                                //The client is native, so this change in how to return the response is for better UX for the end user.
                                //客户端是本地客户端，因此此更改返回响应的方式是为最终用户提供更好的UX。
                                //TODO : 这边跳转往后有时间再写
                            }

                            return Redirect(ReturnUrl);
                        }

                        //请求来自本地页面
                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else if (ReturnUrl.IsNullOrEmpty())
                        {
                            return Redirect("/");
                        }
                        else
                        {
                            // user might have clicked on a malicious link - should be logged
                            throw new Exception(L["invalid_return_URL", ReturnUrl]);
                        }
                    }
                }

                await m_IdsEvent.RaiseAsync(new UserLoginFailureEvent(IdentityString, "invalid credentials", clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, L["invalid_credentials_error_message"]);
            }


            return Page();
        }



        public async Task<IActionResult> OnCancelAsync()
        {
            await Task.CompletedTask;
            return Redirect("/");
        }

        private async Task<ChinoUser> FindUserByIdentityStringAsync(string identityString)
        {
            ChinoUser user = null;
            if (m_AccountConfiguration.CanLoginByEmail() && identityString.IndexOf('@') != -1 )
            {
                user = await m_UserManager.FindByEmailAsync(identityString);
            }

            if(user == null && m_AccountConfiguration.CanLoginByUserName())
            {
                user = await m_UserManager.FindByNameAsync(identityString);
            }

            return user;
        }

    }
}
