using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Dtos.Account.Confirmation;

namespace Chino.IdentityServer.Services.Users
{
    public interface IUserService
    {
        Task<SMSVerificationCodeCacheDto> GetLastSMSVerificationCodeInfo(string userId);
        Task<PaginatedListDto<ChinoUser>> GetUsers(int page = 1, int size = 25, string search = null);

        /// <summary>
        /// 获取 用户短信验证码 缓存Key
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetUserSMSVerificationCodeCacheKey(string userId);

        /// <summary>
        /// 获取用户总数
        /// </summary>
        /// <returns></returns>
        Task<long> GetUsersTotalCount();
        Task SendSMSVerificationCode(string phoneNumber, string phoneDialingCode, ChinoUser user);
        Task SetSMSVerificationCodeInfo(string userId, SMSVerificationCodeCacheDto info);
    }
}
