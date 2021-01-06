using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.Services.Clients;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard
{
    public class ClientsModel : PageModel
    {
        private readonly IClientService m_ClientService;

        [BindProperty]
        public string SearchText { get; set; }

        public PaginatedListDto<IdentityServer4.EntityFramework.Entities.Client> Clients { get; set; }

        public ClientsModel(IClientService clientService)
        {
            this.m_ClientService = clientService;
        }

        public async Task<IActionResult> OnGetAsync(int page = 1, int size = 25, string search = null)
        {
            if (size > 100)
                return BadRequest();
            if (page < 1)
                return BadRequest();

            SearchText = search;
            Clients = await m_ClientService.GetClientsAsync(page, size, search);

            return Page();
        }
    }
}
