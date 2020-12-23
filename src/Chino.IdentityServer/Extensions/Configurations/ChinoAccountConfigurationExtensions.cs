using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Services;

namespace Chino.IdentityServer.Extensions.Configurations
{
    public static class ChinoAccountConfigurationExtensions
    {
        public static bool LoginByUserNameOnly(this ChinoAccountConfiguration configuration)
            => configuration.UserName.Login && !configuration.Email.Login && !configuration.Phone.Login;

        public static bool CanLoginByUserName(this ChinoAccountConfiguration configuration)
            => configuration.UserName.Login;

        public static bool LoginByEmailOnly(this ChinoAccountConfiguration configuration)
            => configuration.Email.Login && !configuration.UserName.Login && !configuration.Phone.Login;

        public static bool CanLoginByEmail(this ChinoAccountConfiguration configuration)
            => configuration.Email.Login;

        public static bool LoginByPhoneOnly(this ChinoAccountConfiguration configuration)
            => configuration.Phone.Login && !configuration.UserName.Login && !configuration.Email.Login;

        public static bool CanLoginByPhone(this ChinoAccountConfiguration configuration)
            => configuration.Phone.Login;

        public static bool RegisterByPhoneNumberOnly(this ChinoAccountConfiguration configuration)
            => configuration.Phone.Register && !configuration.Email.Register && !configuration.UserName.Register;


        /// <summary>
        /// 在注册时候需要确认手机号
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool IsNeedToConfirmPhoneNumberWhenRegister(this ChinoAccountConfiguration configuration)
            => configuration.Phone.Register && configuration.Phone.RegisterRequire && configuration.Phone.RequireConfirmedPhoneNumber;

        /// <summary>
        /// 在注册时需要确认电子邮件
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool IsNeedToConfirmEmailWhenRegister(this ChinoAccountConfiguration configuration)
            => configuration.Email.Register && configuration.Email.RegisterRequire && configuration.Email.RequireConfirmedEmail;

        /// <summary>
        /// 在注册时需要确认电子邮件和手机号
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool IsNeedToConfirmEmailAndPhoneWhenRegister(this ChinoAccountConfiguration configuration)
            => configuration.IsNeedToConfirmEmailWhenRegister() && configuration.IsNeedToConfirmPhoneNumberWhenRegister();

        /// <summary>
        /// 可使用短信验证码登录
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool CanLoginBySMSVerificationCode(this ChinoAccountConfiguration configuration)
            => configuration.Phone.Login && configuration.Phone.SMSLogin;


        /// <summary>
        /// 是否只有一种登录途径
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool LoginByOnlyOne(this ChinoAccountConfiguration configuration)
        {
            List<bool> bools = new List<bool>();
            bools.Add(configuration.LoginByUserNameOnly());
            bools.Add(configuration.LoginByEmailOnly());
            bools.Add(configuration.LoginByPhoneOnly());

            return bools.Where(b => b).Count() == 1;
        }


        /// <summary>
        /// eg: if can login by username only,return :"Username", if can login by username or email , return "Username / Email"
        /// </summary>
        /// <param name="accountConfiguration"></param>
        /// <param name="localizer"></param>
        /// <returns></returns>
        public static string GetLoginInputText(this ChinoAccountConfiguration accountConfiguration, CommonLocalizationService localizer)
        {
            bool flag = false;
            string text = "";
            //UserName
            if (accountConfiguration.UserName.Login)
            {
                text += localizer["username"];
                flag = true;
            }

            //Email
            if (accountConfiguration.Email.Login)
            {
                text += flag ? $"/{localizer["email"]}" : localizer["email"];
                flag = true;
            }

            //Phone
            if (accountConfiguration.Phone.Login)
            {
                text += flag? $"/{localizer["phone_number"]}" : localizer["phone_number"];
                flag = true;
            }


            return text;
        }
    }
}
