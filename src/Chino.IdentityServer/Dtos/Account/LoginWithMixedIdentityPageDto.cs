using System.ComponentModel.DataAnnotations;

namespace Chino.IdentityServer.Dtos.Account
{
    /// <summary>
    /// 混合身份凭证登录授权
    /// </summary>
    public class LoginWithMixedIdentityPageDto
    {
        //[Required(ErrorMessage = "identity_required")]
        //public string Identity { get; set; }

        [Required(ErrorMessage = "passwd_required")]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }
    }
}
