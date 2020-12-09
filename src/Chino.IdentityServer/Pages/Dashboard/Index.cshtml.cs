using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Models.User;
using Chino.IdentityServer.Services.Clients;
using Chino.IdentityServer.Services.Users;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// �û�����
        /// </summary>
        public long UsersCount { get; set; }

        /// <summary>
        /// �ͻ�������
        /// </summary>
        public long ClientsCount { get; set; }

        private readonly IUserService m_UserService;
        private readonly IClientService m_ClientService;

        public IndexModel(IUserService usersService, IClientService clientService)
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