using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nekonya;

namespace Chino.IdentityServer.Pages.Dashboard
{
    public class UsersModel : PageModel
    {
        private readonly IUserService m_UsersService;
        private readonly UserManager<ChinoUser> m_UserManager;

        public PaginatedListDto<ChinoUser> Users { get; set; }
        public Dictionary<ChinoUser, IList<Claim>> UserClaims { get; set; } = new Dictionary<ChinoUser, IList<Claim>>();

        [BindProperty]
        public string SearchText { get; set; }

        public UsersModel(IUserService usersService,
            UserManager<ChinoUser> userManager)
        {
            m_UsersService = usersService;
            m_UserManager = userManager;
        }


        public async Task<IActionResult> OnGet(int page = 1, int size = 25, string search = null)
        {
            if (size > 100)
                return BadRequest();
            if (page < 1)
                return BadRequest();

            SearchText = search;

            Users = await m_UsersService.GetUsers(page, size, search);
            foreach(var user in Users.Data)
            {
                var claims = await m_UserManager.GetClaimsAsync(user);
                UserClaims.AddIfNotExist(user, claims);
            }

            return Page();
        }
    }
}
