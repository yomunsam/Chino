using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Services.Oidc
{
    public class ChinoProfileService : IProfileService
    {
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly ILogger<ChinoProfileService> m_logger;

        public ChinoProfileService(UserManager<ChinoUser> userManager,
            ILogger<ChinoProfileService> logger)
        {
            this.m_UserManager = userManager;
            this.m_logger = logger;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.LogProfileRequest(m_logger);

            //判断是否有请求Claim信息
            if (context.RequestedClaimTypes.Any())
            {
                var user = await m_UserManager.FindByIdAsync(context.Subject.GetSubjectId());
                if(user != null)
                {
                    var claims = await m_UserManager.GetClaimsAsync(user); //获取到所有claims
                    context.AddRequestedClaims(claims); //这个方法内部会在所有的claims里找到用户请求的claim
                }
            }

            context.LogIssuedClaims(m_logger);
        }

        /// <summary>
        /// 验证用户是否被允许获得令牌
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            m_logger.LogDebug("IsActive called from: {caller}", context.Caller);

            var user = await m_UserManager.FindByIdAsync(context.Subject.GetSubjectId());
            if (user is not null)
            {
                if (await m_UserManager.IsLockedOutAsync(user))
                    context.IsActive = false;
                else
                    context.IsActive = true;
            }
            else
                context.IsActive = false;
        }
    }
}
