using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Services.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Nekonya;

namespace Chino.IdentityServer.Pages.Dashboard.User
{
    public class UserRolesModel : PageModel
    {
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly RoleManager<IdentityRole> m_RoleManager;
        private readonly IRoleService m_RoleService;

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        [BindProperty]
        public string AddRoleName { get; set; }

        public ChinoUser UserEntity { get; set; }

        public List<IdentityRole> AllRoles { get; set; } = new List<IdentityRole>();

        public IList<string> UserRoles { get; set; }
        

        public UserRolesModel(UserManager<ChinoUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IRoleService roleService)
        {
            this.m_UserManager = userManager;
            this.m_RoleManager = roleManager;
            this.m_RoleService = roleService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (UserId.IsNullOrEmpty())
                return BadRequest();
            UserEntity = await m_UserManager.FindByIdAsync(UserId);
            if (UserEntity == null)
                return NotFound();

            AllRoles = await m_RoleManager.Roles
                .AsNoTracking()
                .OrderBy(role => role.NormalizedName)
                .ToListAsync();

            UserRoles = await this.m_RoleService.GetUserRolesAsync(UserEntity);

            

            return Page();
        }

        /// <summary>
        /// Ìí¼Ó½ÇÉ«
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (UserId.IsNullOrEmpty())
                return BadRequest();
            UserEntity = await m_UserManager.FindByIdAsync(UserId);
            if (UserEntity == null)
                return NotFound();

            if (AddRoleName.IsNullOrEmpty())
            {
                ModelState.AddModelError(string.Empty, "Add Role Name is null");
                return Page();
            }
            if (!await m_UserManager.IsInRoleAsync(UserEntity, AddRoleName))
            {
                var result = await m_UserManager.AddToRoleAsync(UserEntity, AddRoleName);
                if (!result.Succeeded)
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
                }
            }


            AllRoles = await m_RoleManager.Roles
                .AsNoTracking()
                .OrderBy(role => role.NormalizedName)
                .ToListAsync();

            UserRoles = await this.m_RoleService.GetUserRolesAsync(UserEntity);

            return Page();
        }


        public async Task<IActionResult> OnGetDeleteRoleAsync(string RoleName)
        {
            if (UserId.IsNullOrEmpty())
                return BadRequest();
            UserEntity = await m_UserManager.FindByIdAsync(UserId);
            if (UserEntity == null)
                return NotFound();
            var result = await m_UserManager.RemoveFromRoleAsync(UserEntity, RoleName);

            if (!result.Succeeded)
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
            }

            AllRoles = await m_RoleManager.Roles
                .AsNoTracking()
                .OrderBy(role => role.NormalizedName)
                .ToListAsync();

            UserRoles = await this.m_RoleService.GetUserRolesAsync(UserEntity);

            return Page();
        }

    }
}
