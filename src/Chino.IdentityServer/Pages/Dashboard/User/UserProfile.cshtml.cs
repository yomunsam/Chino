using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nekonya;

namespace Chino.IdentityServer.Pages.Dashboard.User
{
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<ChinoUser> m_UserManager;

        [BindProperty(SupportsGet = true)]
        public string UserId { get; set; }

        [BindProperty]
        public ChinoUser UserEntity { get; set; }


        public UserProfileModel(UserManager<ChinoUser> userManager)
        {
            this.m_UserManager = userManager;
        }

        public async Task<IActionResult> OnGet()
        {
            if (UserId.IsNullOrEmpty())
                return BadRequest();
            UserEntity = await m_UserManager.FindByIdAsync(UserId);
            if (UserEntity == null)
                return NotFound();

            return Page();
        }
    }
}
