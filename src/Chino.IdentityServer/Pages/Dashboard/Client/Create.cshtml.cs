using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Exceptions.Common;
using Chino.IdentityServer.Services.Clients;
using Chino.IdentityServer.ViewModels.Dashboard.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Chino.IdentityServer.Pages.Dashboard.Client
{
    /// <summary>
    /// 创建客户端的Model
    /// </summary>
    public class CreateModel : PageModel
    {
        private readonly IClientService m_ClientService;
        private readonly IStringLocalizer<CreateModel> L;

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public CreateClientViewModel ViewModel { get; set; }


        public CreateModel(IClientService clientService,
            IStringLocalizer<CreateModel> localizer)
        {
            this.m_ClientService = clientService;
            this.L = localizer;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = await m_ClientService.CreateClient(ViewModel.ClientId, ViewModel.ClientName, ViewModel.Description);
                return LocalRedirect(this.ReturnUrl ?? "~/");
            }
            catch(AlreadyExists)
            {
                this.ModelState.AddModelError(string.Empty, L["id_already_exists", ViewModel.ClientId]);
                return Page();
            }
        }
    }
}
