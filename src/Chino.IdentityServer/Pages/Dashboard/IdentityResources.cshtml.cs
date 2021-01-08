using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.Services.IdentityResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard
{
    public class IdentityResourcesModel : PageModel
    {
        private readonly IIdentityResouceService m_IdentityResouceService;

        [BindProperty]
        public string SearchText { get; set; }

        public PaginatedListDto<IdentityServer4.EntityFramework.Entities.IdentityResource> IdentityResource { get; set; }

        public IdentityResourcesModel(IIdentityResouceService identityResouceService)
        {
            this.m_IdentityResouceService = identityResouceService;
        }

        public async Task<IActionResult> OnGetAsync(int page = 1, int size = 25, string search = null)
        {
            if (size > 100)
                return BadRequest();
            if (page < 1)
                return BadRequest();


            SearchText = search;
            IdentityResource = await m_IdentityResouceService.GetIdentityResourcesAsync(page, size, search);

            return Page();
        }
    }
}
