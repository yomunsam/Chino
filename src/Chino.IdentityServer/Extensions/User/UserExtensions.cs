using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;

namespace Chino.IdentityServer.Extensions.User
{
    
    public static class UserExtensions
    {
        public static string GetName(this IList<Claim> claims)
        {
            string name = claims.Where(c => c.Type == JwtClaimTypes.Name).FirstOrDefault()?.Value;
            return name;
        }

        public static string GetNickName(this IList<Claim> claims)
        {
            string nickName = claims.Where(c => c.Type == JwtClaimTypes.NickName).FirstOrDefault()?.Value;
            return nickName;
        }

        public static string GetNickName(this ClaimsPrincipal principal)
        {
            var nick = principal.FindFirst(JwtClaimTypes.NickName);
            return nick?.Value;
        }

    }
}
