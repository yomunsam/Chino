using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chino.IdentityServer.Const
{
    public static class ConfigurationKeyConst
    {
        /// <summary>
        /// 可以访问Chino后台的角色名
        /// </summary>
        public const string ChinoAdminRoleName = "Chino:AdminRoleName";

        /// <summary>
        /// 是否启用Gravatar
        /// </summary>
        public const string EnableGravatar = "Gravatar:Enable";

        public const string GravatarDefaultImageType = "Gravatar:DefaultAvatar:Type";
        public const string GravatarDefaultImageUrl = "Gravatar:DefaultAvatar:Url";

        /// <summary>
        /// 短信服务提供者
        /// </summary>
        public const string SMSProvider = "SMS:Provider";

        /// <summary>
        /// 发送短信验证码的间隔
        /// </summary>
        public const string SendSMSVerificationCodeInterval = "SMS:SendSMSVerificationCodeInterval";
    }
}
