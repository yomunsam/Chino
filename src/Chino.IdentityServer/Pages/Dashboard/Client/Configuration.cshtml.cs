using System.Threading.Tasks;
using AutoMapper;
using Chino.IdentityServer.Services.Clients;
using Chino.IdentityServer.ViewModels.Dashboard.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard.Client
{
    public class ConfigurationModel : PageModel
    {
        private readonly IClientService m_ClientService;
        private readonly IMapper m_Mapper;

        [BindProperty(SupportsGet = true)]
        public int ClientId { get; set; }

        [BindProperty]
        public ConfigurationClientViewModel ClientViewModel { get;set; }

        public ConfigurationModel(IClientService clientService,
            IMapper mapper)
        {
            this.m_ClientService = clientService;
            this.m_Mapper = mapper;
        }



        public async Task<IActionResult> OnGetAsync()
        {
            var clientEntity = await m_ClientService.GetClientAsync(ClientId);
            if (clientEntity == null)
                return NotFound();

            this.ClientViewModel = m_Mapper.Map<ConfigurationClientViewModel>(clientEntity);
            return Page();
        }



        public async Task<IActionResult> OnPostAsync(string button)
        {
            if(button == "save")
            {
                var clientEntity = await m_ClientService.UpdateClient(this.ClientId, this.ClientViewModel);
                this.ClientViewModel = m_Mapper.Map<ConfigurationClientViewModel>(clientEntity);
            }


            return Page();
        }

    }
}
