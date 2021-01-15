using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nekonya;

namespace Chino.IdentityServer.Utils.Gravatar
{
    public static class GravatarUtil
    {
        /// <summary>
        /// 获取头像图片Url
        /// </summary>
        /// <param name="email"></param>
        /// <param name="defaultImageUrl"></param>
        /// <returns></returns>
        public static string GetImageUrl(string email, int? size = null, string defaultValue = null)
        {
            var email_hash = email.Trim()
                .ToLower()
                .GetMD5(true, false);
            var finalUrl = $"//www.gravatar.com/avatar/{email_hash}";

            bool addParamFlag = false; //如果有在url上添加过"?"符号了，就置为true

            if(size != null)
            {
                finalUrl += $"?s={size}";
                addParamFlag = true;
            }

            if(defaultValue is not null)
            {
                if (addParamFlag)
                    finalUrl += $"&d={defaultValue}";
                else
                    finalUrl += $"?d={defaultValue}";

                addParamFlag = true;
            }

            return finalUrl;
        }
    }
}
