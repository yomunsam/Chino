using System;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Services.Users;
using Chino.IdentityServer.ViewModels.Account.Confirmation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nekonya;

namespace Chino.IdentityServer.Pages.Account.Confirmation
{
    public class PhoneModel : PageModel
    {
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly SignInManager<ChinoUser> m_SignInManager;
        private readonly IUserService m_UserService;

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }



        [BindProperty]
        public PhoneInputModel InputModel { get; set; }

        public ChinoUser TargetUser { get; set; }

        public PhoneModel(UserManager<ChinoUser> userManager,
            SignInManager<ChinoUser> signInManager,
            IUserService userService)
        {
            this.m_UserManager = userManager;
            this.m_SignInManager = signInManager;
            this.m_UserService = userService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            if (UserId.IsNullOrEmpty())
                return BadRequest();
            InputModel = await BuildInputModel();
            if (TargetUser == null)
                return NotFound();


            return Page();
        }

        /// <summary>
        /// 提交验证发和请求发送验证码都在这儿
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string button)
        {
            if (UserId.IsNullOrEmpty())
                return BadRequest();
            var userInfo = await m_UserManager.FindByIdAsync(this.UserId);
            if (userInfo == null)
                return NotFound();

            if (button == "sendVerificationCode") //发送验证码流程
            {
                var cacheInfo = await m_UserService.GetLastSMSVerificationCodeInfo(this.UserId);
                if(cacheInfo != null)
                {
                    ModelState.AddModelError(string.Empty, "请等待XX秒后再试");
                    return Page();
                }

                if(InputModel.PhoneNumber.IsNullOrEmpty() || InputModel.PhoneDialingCode.IsNullOrEmpty())
                {
                    ModelState.AddModelError(string.Empty, "电话号码或国家代码无效");
                    return Page();
                }

                await m_UserService.SendSMSVerificationCode(InputModel.PhoneNumber, InputModel.PhoneDialingCode, userInfo);

            }

            if(ModelState.IsValid)
            {
                if(button == "confirm") //提交验证码并登录
                {
                    if(await m_UserManager.VerifyChangePhoneNumberTokenAsync(userInfo, InputModel.VerificationCode, InputModel.PhoneNumber))
                    {
                        Console.WriteLine("验证通过");
                        await m_SignInManager.SignInAsync(userInfo, true);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else if (ReturnUrl.IsNullOrEmpty())
                        {
                            return Redirect("/");
                        }
                    }
                }
            }


            return Page();
        }

        private async Task<PhoneInputModel> BuildInputModel()
        {
            TargetUser = await m_UserManager.FindByIdAsync(this.UserId);
            if (TargetUser == null)
                return null;

            var result = new PhoneInputModel();

            if (!TargetUser.PhoneNumber.IsNullOrEmpty())
                result.PhoneNumber = TargetUser.PhoneNumber;

            if (!TargetUser.PhoneDialingCode.IsNullOrEmpty())
                result.PhoneDialingCode = TargetUser.PhoneDialingCode;


            return result;
        }

    }
}
