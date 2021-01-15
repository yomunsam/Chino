using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Const;
using Chino.IdentityServer.Utils.Gravatar;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Chino.IdentityServer.Services.Users
{
    /// <summary>
    /// 用户形象（头像）服务
    /// </summary>
    public class UserAvatarService : IUserAvatarService
    {
        /// <summary>
        /// 本地的默认用户头像Url
        /// </summary>
        public const string c_DefaultUserAvatarUrl = @"~/img/default_avatar.png";

        private readonly IConfiguration m_Configuration;
        private readonly UserManager<ChinoUser> m_UserManager;

        private bool m_EnableGravatar;


        private ChinoUser m_CurrentUser { get; set; }

        public UserAvatarService(IConfiguration configuration,
            UserManager<ChinoUser> userManager
            )
        {
            this.m_Configuration = configuration;
            this.m_UserManager = userManager;
            m_EnableGravatar = m_Configuration.GetValue<bool>(ConfigurationKeyConst.EnableGravatar);
        }


        /// <summary>
        /// 获取用户头像Url
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCurrentUserAvatarUrl(ClaimsPrincipal userClaimsPrincipal, HttpRequest httpRequest, int? size = null)
        {
            if(m_CurrentUser is null)
            {
                m_CurrentUser = await m_UserManager.GetUserAsync(userClaimsPrincipal);
            }

            if (m_EnableGravatar)
            {
                return GravatarUtil.GetImageUrl(m_CurrentUser.Email, size, this.GetDefaultImageUrl($"{httpRequest.Scheme}://{httpRequest.Host}/"));
            }
            else
            {
                return c_DefaultUserAvatarUrl.Replace("~/",$"/");
            }
        }


        private string GetDefaultImageUrl(string hostBaseUrl)
        {
            var type = m_Configuration[ConfigurationKeyConst.GravatarDefaultImageType]?.ToLower() ?? "identicon";

            if(type == "url")
            {
                var url = m_Configuration[ConfigurationKeyConst.GravatarDefaultImageUrl] ?? c_DefaultUserAvatarUrl;
                if (url.StartsWith("~/"))
                    url = url.Replace("~/", hostBaseUrl);
                
                return WebUtility.UrlEncode(url);
            }

            return type;
        }

    }
}
