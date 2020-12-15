using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Chino.IdentityServer.Services.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Nekonya;

namespace Chino.IdentityServer.Pages.Dashboard.Role
{
    [Authorize]
    public class AddModel : PageModel
    {
        private readonly IStringLocalizer<AddModel> L;
        private readonly IRoleService m_RoleService;


        [BindProperty]
        [Required(ErrorMessage = "rolename_required")]
        public string AddRoleName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public AddModel(IStringLocalizer<AddModel> localizer,
            IRoleService roleService)
        {
            this.L = localizer;
            m_RoleService = roleService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var result = await m_RoleService.CreateRole(AddRoleName);
            if (result.Succeeded)
            {
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                foreach(var error in result.Errors)
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
