using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.Models.User;

namespace Chino.IdentityServer.Services.Users
{
    public interface IUserService
    {
        Task<PaginatedListDto<ChinoUser>> GetUsers(int page = 1, int size = 25, string search = null);
        
        /// <summary>
        /// 获取用户总数
        /// </summary>
        /// <returns></returns>
        Task<long> GetUsersTotalCount();
    }
}
