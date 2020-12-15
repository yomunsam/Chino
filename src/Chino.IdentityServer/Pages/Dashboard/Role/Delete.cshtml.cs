using System.Threading.Tasks;
using Chino.IdentityServer.Services.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard.Role
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IRoleService m_RoleService;

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public string RoleId { get; set; }

        [BindProperty]
        public string RoleName { get; set; }

        public DeleteModel(IRoleService roleService)
        {
            this.m_RoleService = roleService;
        }

        public void OnGet(string roleName, string roleId)
        {
            this.RoleName = roleName;
            this.RoleId = roleId;
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var result = await m_RoleService.DeleteRole(this.RoleId);
            if (result.Succeeded)
            {
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        default:
                            ModelState.AddModelError(string.Empty, error.Description);
                            break;
                    }
                }
                return Page();
            }
        }

    }
}
