using System;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.Utils.PaginatedList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nekonya;

namespace Chino.IdentityServer.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ChinoUser> m_UserManager;
        public UserService(UserManager<ChinoUser> userManager)
        {
            m_UserManager = userManager;
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

    }
}
