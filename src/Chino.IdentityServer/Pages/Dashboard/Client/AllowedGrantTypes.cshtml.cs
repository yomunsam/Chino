using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Pages.Dashboard.Client
{
    public class AllowedGrantTypesModel : PageModel
    {
        private readonly IClientService m_ClientService;
        private readonly ILogger<AllowedGrantTypesModel> m_Logger;


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IdentityServer4.EntityFramework.Entities.Client ClientEntity { get; set; }

        public List<string> SuggestedItem;

        public AllowedGrantTypesModel(IClientService clientService,
            ILogger<AllowedGrantTypesModel> logger)
        {
            this.m_ClientService = clientService;
            this.m_Logger = logger;

            SuggestedItem = new List<string>
            {
                "authorization_code",       //授权码
                "client_credentials",
                "refresh_token",
                "implicit",                 //隐式
                "password",
                "urn:ietf:params:oauth:grant-type:device_code" //设备代码流
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this.ClientEntity = await m_ClientService.GetClientAndAllowedGrantTypes(this.Id);
            if (ClientEntity == null)
                return NotFound();

            foreach(var item in ClientEntity.AllowedGrantTypes)
            {
                if (SuggestedItem.Contains(item.GrantType))
                    SuggestedItem.Remove(item.GrantType);
            }
            return Page();
        }

        
    }
}
