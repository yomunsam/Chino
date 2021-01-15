using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Chino.IdentityServer.Services.Users
{
    /// <summary>
    /// 用户形象（头像）服务接口
    /// </summary>
    public interface IUserAvatarService
    {
        Task<string> GetCurrentUserAvatarUrl(ClaimsPrincipal userClaimsPrincipal, HttpRequest httpRequest, int? size = null);
    }
}
