using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Chino.IdentityServer.Services.Clients;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard.Client
{
    public class AllowedScopesModel : PageModel
    {
        private readonly IClientService m_ClientService;
        private readonly IMapper m_Mapper;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public IdentityServer4.EntityFramework.Entities.Client ClientEntity { get; set; }

        public AllowedScopesModel(IClientService clientService,
            IMapper mapper)
        {
            this.m_ClientService = clientService;
            this.m_Mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ClientEntity = await m_ClientService.GetClientAsync(this.Id);
            if (ClientEntity == null)
                return NotFound();

            return Page();
        }
    }
}
