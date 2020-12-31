namespace Chino.IdentityServer.Services.Account
{
    public interface IAccountService
    {
        /// <summary>
        /// 是否开放注册账号
        /// </summary>
        bool EnableRegister { get; }

        /// <summary>
        /// 可否使用手机号来登录
        /// </summary>
        bool EnableLoginByPhone { get; }

        /// <summary>
        /// 可否使用短信验证码来登录
        /// </summary>
        bool EnableLoginBySMSVerificationCode { get; }

        string GetLoginLabelText(CommonLocalizationService localizer, bool NoPhoneNumber = true);
    }
}
