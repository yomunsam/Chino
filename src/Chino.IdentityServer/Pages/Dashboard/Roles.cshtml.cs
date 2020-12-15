using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.Services.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard
{
    [Authorize]
    public class RolesModel : PageModel
    {
        private readonly IRoleService m_RoleService;

        [BindProperty]
        public string SearchText { get; set; }

        public PaginatedListDto<IdentityRole> Roles { get; set; }

        public RolesModel(IRoleService roleService)
        {
            m_RoleService = roleService;
        }


        public async Task<IActionResult> OnGet(int page = 1, int size = 25, string search = null)
        {
            if (size > 100)
                return BadRequest();
            if (page < 1)
                return BadRequest();

            SearchText = search;
            Roles = await m_RoleService.GetRolesAsync(page, size, search);

            return Page();
        }
    }
}
