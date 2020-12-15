using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.Utils.PaginatedList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nekonya;

namespace Chino.IdentityServer.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> m_RoleManager;
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            m_RoleManager = roleManager;
        }

        public async Task<PaginatedListDto<IdentityRole>> GetRolesAsync(int page = 1, int size = 25, string search = null)
        {
            
            var source = m_RoleManager.Roles
                .OrderBy(role => role.Id)
                .AsNoTracking();

            if (!search.IsNullOrEmpty())
                source = source.Where(role => role.Name.ToUpper().Contains(search.ToUpper())
                || role.Id.ToUpper().Contains(search.ToUpper()));

            var result = await PaginatedList<IdentityRole>.CreateAsync(source, page, size);
            return result.GetDto();
        }


        /// <summary>
        /// if role name not exists, return true, add to database.
        /// either return false;
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateRole(string roleName)
        {
            var role = new IdentityRole(roleName);
            return m_RoleManager.CreateAsync(role);
        }


        public async Task<IdentityResult> DeleteRole(string roleId)
        {
            var role = await m_RoleManager.FindByIdAsync(roleId);
            return await m_RoleManager.DeleteAsync(role);
        }


        public Task<long> GetRolesTotalCount()
        {
            return m_RoleManager.Roles.LongCountAsync();
        }

    }
}
