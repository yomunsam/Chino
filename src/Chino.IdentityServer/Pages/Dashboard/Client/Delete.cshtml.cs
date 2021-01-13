using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Exceptions.Common;
using Chino.IdentityServer.Services.Clients;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Pages.Dashboard.Client
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> m_Logger;
        private readonly IClientService m_ClientService;

        public DeleteModel(ILogger<DeleteModel> logger,
            IClientService clientService)
        {
            this.m_Logger = logger;
            this.m_ClientService = clientService;
        }


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IdentityServer4.EntityFramework.Entities.Client ClientEntity { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            ClientEntity = await m_ClientService.GetClientAsync(this.Id);
            if (ClientEntity == null)
                return NotFound();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = await m_ClientService.DeleteClientById(this.Id);
                m_Logger.LogInformation("Client \"{0}\" (clientID:{1}, description:{2}) was deleted by user \"{3}\"({4})",
                    client.ClientName,
                    client.ClientId,
                    client.Description,
                    this.User.GetSubjectId(),
                    this.User.GetDisplayName());
                return LocalRedirect(this.ReturnUrl ?? "~/");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }


    }
}
