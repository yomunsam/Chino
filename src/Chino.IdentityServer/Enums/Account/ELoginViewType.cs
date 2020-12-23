namespace Chino.IdentityServer.Enums.Account
{
    /// <summary>
    /// 登录视图的登录类型
    /// </summary>
    public enum ELoginViewType : int
    {
        /// <summary>
        /// 用户名或邮箱方式
        /// </summary>
        UserNameOrEmail = 0,
        /// <summary>
        /// 手机号加口令方式
        /// </summary>
        PhoneAndPassword = 1,
        /// <summary>
        /// 短信验证码方式
        /// </summary>
        SMSVerificationCode = 2,
    }
}
