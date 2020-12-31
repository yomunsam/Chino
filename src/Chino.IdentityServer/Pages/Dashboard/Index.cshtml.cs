using System.Threading.Tasks;
using Chino.IdentityServer.Services.Clients;
using Chino.IdentityServer.Services.Localization;
using Chino.IdentityServer.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// 用户总数
        /// </summary>
        public long UsersCount { get; set; }

        /// <summary>
        /// 客户端总数
        /// </summary>
        public long ClientsCount { get; set; }

        private readonly IUserService m_UserService;
        private readonly IClientService m_ClientService;

        public IndexModel(IUserService usersService, IClientService clientService, IJsonLocalizationService jl)
        {
            m_UserService = usersService;
            m_ClientService = clientService;

        }


        public async Task<IActionResult> OnGet()
        {
            UsersCount = await m_UserService.GetUsersTotalCount();
            ClientsCount = await m_ClientService.GetClientsTotalCount();
            return Page();
        }
    }
}
