using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Const;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Chino.IdentityServer.Pages.User
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<ChinoUser> m_UserManager;
        private readonly IConfiguration m_Configuration;

        public bool AllowDelete { get; set; } = true;

        public PersonalDataModel(UserManager<ChinoUser> userManager, 
            IConfiguration configuration)
        {
            this.m_UserManager = userManager;
            this.m_Configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await m_UserManager.GetUserAsync(this.User);
            if(await m_UserManager.IsInRoleAsync(user, m_Configuration[ConfigurationKeyConst.ChinoAdminRoleName]))
            {
                AllowDelete = false;
            }


            return Page();
        }
    }
}
