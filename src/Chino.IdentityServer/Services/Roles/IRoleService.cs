using System.Collections.Generic;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.EntityFramework.Shared.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Chino.IdentityServer.Services.Roles
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRole(string roleName);
        Task<IdentityResult> DeleteRole(string roleId);
        Task<PaginatedListDto<IdentityRole>> GetRolesAsync(int page = 1, int size = 25, string search = null);
        Task<IList<string>> GetUserRolesAsync(ChinoUser user);
    }
}
