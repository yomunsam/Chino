using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Dtos.Account;
using Chino.IdentityServer.Extensions.Configurations;
using Chino.IdentityServer.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Nekonya;
using Nekonya.Utils.String;

namespace Chino.IdentityServer.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ChinoAccountConfiguration m_AccountConfiguration;
        private readonly CountryCodeConfiguration m_CountryCode;
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly SignInManager<ChinoUser> m_SignInManager;
        private readonly IStringLocalizer<RegisterModel> L;

        public RegisterModel(ChinoAccountConfiguration chinoAccountConfiguration,
            CountryCodeConfiguration countryCodeConfiguration,
            UserManager<ChinoUser> userManager,
            SignInManager<ChinoUser> signInManager,
            IStringLocalizer<RegisterModel> localizer)
        {
            m_AccountConfiguration = chinoAccountConfiguration;
            m_UserManager = userManager;
            m_SignInManager = signInManager;
            L = localizer;
            m_CountryCode = countryCodeConfiguration;
        }

        [BindProperty]
        public RegisterInputModel RegisterDto { get; set; }

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

            //根据语言尝试推断默认的手机号国家码
            var locale = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var current_culture = locale.RequestCulture?.UICulture;
            if(current_culture != null)
            {
                if (m_CountryCode.LCID_Dict.TryGetValue(current_culture.LCID, out var country))
                {
                    if (RegisterDto == null)
                        RegisterDto = new RegisterInputModel();
                    RegisterDto.PhoneDialingCode = country.DialingCodeWithoutPlus;
                }
            }


            return Page();
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            
            
            if (m_AccountConfiguration.Phone.Register)
            {
                if (!RegisterDto.PhoneDialingCode.IsNullOrEmpty())
                {
                    if (RegisterDto.PhoneDialingCode.StartsWith('+'))
                        RegisterDto.PhoneDialingCode = RegisterDto.PhoneDialingCode.Substring(1, RegisterDto.PhoneDialingCode.Length - 1);
                }

                if (m_AccountConfiguration.Phone.RegisterRequire)
                {
                    //校验DialingCode是否存在
                    if (!m_CountryCode.DialingCodeWithoutPlus_Dict.ContainsKey(RegisterDto.PhoneDialingCode))
                    {
                        ModelState.AddModelError(string.Empty, L["invalid_dialingCode", RegisterDto.PhoneDialingCode]);
                        return Page();
                    }
                }
            }
            

            var user = new ChinoUser
            {
                UserName = RegisterDto.UserName,
                Email = RegisterDto.Email,
                PhoneNumber = string.Format("{0}{1}",RegisterDto.PhoneDialingCode, RegisterDto.PhoneNumber),
                //PhoneDialingCode = RegisterDto.PhoneDialingCode,
            };

            if (m_AccountConfiguration.RegisterByPhoneNumberOnly())
            {
                //仅通过手机号登录
                if(user.UserName.IsNullOrEmpty())
                {
                    user.UserName = await this.GetAvailableUserNameByPhoneNumber("tel", RegisterDto.PhoneNumber);
                }
            }

            //if (!m_AccountConfiguration.UserName.Register)
            //    user.UserName = RegisterDto.Email;

            var result = await m_UserManager.CreateAsync(user, RegisterDto.Password);
            if (result.Succeeded)
            {
                //账号验证

                if (m_AccountConfiguration.IsNeedToConfirmEmailAndPhoneWhenRegister())
                {
                    //Todo: 同时需要确认Email和手机号的情况
                }
                else
                {
                    if (m_AccountConfiguration.IsNeedToConfirmEmailWhenRegister())
                    {
                        //Todo： 需要确电子邮件认
                    }

                    if(m_AccountConfiguration.IsNeedToConfirmPhoneNumberWhenRegister())
                    {
                        //需要确认手机号
                        var code = await m_UserManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

                        //发送验证码

                        //重定向
                        return RedirectToPage("/Account/Confirmation/Phone", new { UserId = user.Id});
                    }
                }


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



        /// <summary>
        /// 根据手机号生成一个可用的（尚未被注册的）用户名字符串
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        private async Task<string> GetAvailableUserNameByPhoneNumber(string prefix, string phoneNumber)
        {
            string userName = string.Format("{0}{1}", prefix, phoneNumber);
            while ((await m_UserManager.FindByNameAsync(userName)) != null)
            {
                userName = string.Format("{0}{1}{2}", prefix, phoneNumber, StringUtil.GetRandom(4));
            }
            return userName;
        }



    }
}
