using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Chino.IdentityServer.ViewModels.Account
{
    public class PhoneLoginInputModel
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Phone(ErrorMessage = "invalid_phoneNumber")]
        [Required(ErrorMessage = "phone_number_required")]
        public string PhoneNumber { get; set; }
        
        /// <summary>
        /// 手机号国家代码
        /// </summary>
        [Required(ErrorMessage = "phone_number_dialing_code_required")]
        public string PhoneDialingCode { get; set; }

        [Required(ErrorMessage = "passwd_required")]
        [PasswordPropertyText]
        public string Password { get; set; }

        /// <summary>
        /// 记住登录
        /// </summary>
        public bool RememberLogin { get; set; }
    }
}
