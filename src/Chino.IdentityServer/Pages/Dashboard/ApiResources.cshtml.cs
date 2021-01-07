using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.Services.ApiResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard
{
    public class ApiResourcesModel : PageModel
    {
        private readonly IApiResourceService m_ApiResourceService;

        [BindProperty]
        public string SearchText { get; set; }

        public PaginatedListDto<IdentityServer4.EntityFramework.Entities.ApiResource> ApiResources { get; set; }

        public ApiResourcesModel(IApiResourceService apiResourceService)
        {
            this.m_ApiResourceService = apiResourceService;
        }

        public async Task<IActionResult> OnGetAsync(int page = 1, int size = 25, string search = null)
        {
            if (size > 100)
                return BadRequest();
            if (page < 1)
                return BadRequest();

            SearchText = search;
            ApiResources = await m_ApiResourceService.GetApiResourcesAsync(page, size, search);

            return Page();
        }
    }
}
