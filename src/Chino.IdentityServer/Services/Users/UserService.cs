using System;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Const;
using Chino.IdentityServer.Dtos.Account.Confirmation;
using Chino.SMS.Shared;
using Chino.Utils.PaginatedList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Nekonya;

namespace Chino.IdentityServer.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly IChinoSMSService m_ChinoSMSService;
        private readonly IMemoryCache m_Cache;
        
        /// <summary>
        /// 发送短信验证码的缓存设置
        /// （延迟时间，在上一个缓存过期之前不允许发送新的验证码）
        /// </summary>
        private MemoryCacheEntryOptions m_SendSMSVerificationCodeCacheOption;

        public UserService(UserManager<ChinoUser> userManager,
            IConfiguration configuration,
            IChinoSMSService chinoSMSService1,
            IMemoryCache memoryCache)
        {
            m_UserManager = userManager;
            this.m_ChinoSMSService = chinoSMSService1;
            this.m_Cache = memoryCache;

            if (!int.TryParse(configuration[ConfigurationKeyConst.SendSMSVerificationCodeInterval], out int SMSVerificationCodeIntervalTime))
                SMSVerificationCodeIntervalTime = 120;

            m_SendSMSVerificationCodeCacheOption = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(SMSVerificationCodeIntervalTime));
        }


        public async Task<PaginatedListDto<ChinoUser>> GetUsers(int page = 1, int size = 25, string search = null)
        {
            var source = m_UserManager.Users
                .OrderBy(user => user.Id)
                .AsNoTracking();

            if (!search.IsNullOrEmpty())
                source = source.Where(user => user.UserName.ToUpper().Contains(search.ToUpper()) 
                || user.Email.ToUpper().Contains(search.ToUpper()) 
                || user.Id.ToUpper().Contains(search.ToUpper()));

            var result = await PaginatedList<ChinoUser>.CreateAsync(source, page, size);
            return result.GetDto();
        }

        /// <summary>
        /// 获取用户总数
        /// </summary>
        /// <returns></returns>
        public Task<long> GetUsersTotalCount()
        {
            return m_UserManager.Users.LongCountAsync();
        }

        /// <summary>
        /// 获取 用户短信验证码 缓存Key
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserSMSVerificationCodeCacheKey(string userId)
        {
            return $"sms_verification _code&{userId}";
        }

        public async Task<SMSVerificationCodeCacheDto> GetLastSMSVerificationCodeInfo(string userId)
        {
            await Task.Yield();
            var cache_key = this.GetUserSMSVerificationCodeCacheKey(userId);
            if (m_Cache.TryGetValue<SMSVerificationCodeCacheDto>(cache_key, out var value))
                return value;
            else
                return null;
        }

        public async Task SetSMSVerificationCodeInfo(string userId, SMSVerificationCodeCacheDto info)
        {
            await Task.Yield();
            var cache_key = this.GetUserSMSVerificationCodeCacheKey(userId);
            m_Cache.Set<SMSVerificationCodeCacheDto>(cache_key, info, m_SendSMSVerificationCodeCacheOption);
        }


        public async Task SendSMSVerificationCode(string phoneNumber, string phoneDialingCode, ChinoUser user)
        {
            var token = await m_UserManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);

            await m_ChinoSMSService.SendVerificationCode(token, phoneNumber);
        }

    }
}
