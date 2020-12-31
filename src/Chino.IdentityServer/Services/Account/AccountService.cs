using AutoMapper;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Extensions.Configurations;

namespace Chino.IdentityServer.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly ChinoAccountConfiguration m_AccountConfiguration;

        public AccountService(ChinoAccountConfiguration chinoAccountConfiguration,
            IMapper mapper)
        {
            this.m_AccountConfiguration = mapper.Map<ChinoAccountConfiguration>(chinoAccountConfiguration);
            this.Rationalization(m_AccountConfiguration); //让配置项变得合理
        }

        /// <summary>
        /// 开放注册
        /// </summary>
        public bool EnableRegister => m_AccountConfiguration.EnableRegister;

        /// <summary>
        /// 开放使用手机号登录
        /// </summary>
        public bool EnableLoginByPhone => m_AccountConfiguration.CanLoginByPhone();

        /// <summary>
        /// 可否使用短信验证码来登录
        /// </summary>
        public bool EnableLoginBySMSVerificationCode => m_AccountConfiguration.CanLoginBySMSVerificationCode();

        public string GetLoginLabelText(CommonLocalizationService localizer, bool NoPhoneNumber = true) => m_AccountConfiguration.GetLoginInputText(localizer, NoPhoneNumber);

        /// <summary>
        /// 让配置合理化
        /// </summary>
        public void Rationalization(ChinoAccountConfiguration conf)
        {
            //UserName
            if (!conf.UserName.Register)
                conf.UserName.RegisterRequire = false; //注册时候不需要填写用户名的话，自然不会“注册时候必须要有用户名”

            //Email
            if (!conf.Email.Register)
            {
                conf.Email.RegisterRequire = false;
                conf.Email.RequireConfirmedEmail = false; //不应该要验证邮箱
            }
        }
    }
}
