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
                //TODO: �����ô���ҳ֮�������������ת������ҳ��
                return Redirect("/");
            }
            this.ReturnUrl = returnUrl ?? Url.Content("~/");

            //�������Գ����ƶ�Ĭ�ϵ��ֻ��Ź�����
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
        /// ע��
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
                    //У��DialingCode�Ƿ����
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
                //��ͨ���ֻ��ŵ�¼
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
                //�˺���֤

                if (m_AccountConfiguration.IsNeedToConfirmEmailAndPhoneWhenRegister())
                {
                    //Todo: ͬʱ��Ҫȷ��Email���ֻ��ŵ����
                }
                else
                {
                    if (m_AccountConfiguration.IsNeedToConfirmEmailWhenRegister())
                    {
                        //Todo�� ��Ҫȷ�����ʼ���
                    }

                    if(m_AccountConfiguration.IsNeedToConfirmPhoneNumberWhenRegister())
                    {
                        //��Ҫȷ���ֻ���
                        var code = await m_UserManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

                        //������֤��

                        //�ض���
                        return RedirectToPage("/Account/Confirmation/Phone", new { UserId = user.Id});
                    }
                }


                await m_SignInManager.SignInAsync(user, false);
                return LocalRedirect(ReturnUrl);
            }

            //���ɹ�
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
        /// �����ֻ�������һ�����õģ���δ��ע��ģ��û����ַ���
        /// </summary>
        /// <param name="prefix">ǰ׺</param>
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
