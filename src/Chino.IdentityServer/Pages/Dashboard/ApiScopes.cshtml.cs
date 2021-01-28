using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.Services.ApiScopes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard
{
    /// <summary>
    /// ApiScopes 总列表
    /// </summary>
    public class ApiScopesModel : PageModel
    {
        private readonly IApiScopeService m_ApiScopeService;

        [BindProperty]
        public string SearchText { get; set; }

        public PaginatedListDto<IdentityServer4.EntityFramework.Entities.ApiScope> ApiScopes { get; set; }

        public ApiScopesModel(IApiScopeService apiScopeService)
        {
            this.m_ApiScopeService = apiScopeService;
        }

        public async Task<IActionResult> OnGetAsync(int page = 1, int size = 25, string search = null)
        {
            if (size > 100)
                return BadRequest();
            if (page < 1)
                return BadRequest();

            SearchText = search;
            ApiScopes = await m_ApiScopeService.GetListAsync(page, size, search);

            return Page();
        }
    }
}
