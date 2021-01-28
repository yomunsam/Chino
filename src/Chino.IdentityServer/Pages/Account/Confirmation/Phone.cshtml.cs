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
        /// �ύ��֤������������֤�붼�����
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync(string button)
        {
            if (UserId.IsNullOrEmpty())
                return BadRequest();
            var userInfo = await m_UserManager.FindByIdAsync(this.UserId);
            if (userInfo == null)
                return NotFound();

            if (button == "sendVerificationCode") //������֤������
            {
                var cacheInfo = await m_UserService.GetLastSMSVerificationCodeInfo(this.UserId);
                if(cacheInfo != null)
                {
                    ModelState.AddModelError(string.Empty, "��ȴ�XX�������");
                    return Page();
                }

                if(InputModel.PhoneNumber.IsNullOrEmpty() || InputModel.PhoneDialingCode.IsNullOrEmpty())
                {
                    ModelState.AddModelError(string.Empty, "�绰�������Ҵ�����Ч");
                    return Page();
                }

                await m_UserService.SendSMSVerificationCode(InputModel.PhoneNumber, InputModel.PhoneDialingCode, userInfo);

            }

            if(ModelState.IsValid)
            {
                if(button == "confirm") //�ύ��֤�벢��¼
                {
                    if(await m_UserManager.VerifyChangePhoneNumberTokenAsync(userInfo, InputModel.VerificationCode, InputModel.PhoneNumber))
                    {
                        Console.WriteLine("��֤ͨ��");
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
