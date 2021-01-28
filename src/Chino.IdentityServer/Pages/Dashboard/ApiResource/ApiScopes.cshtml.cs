using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chino.IdentityServer.Services.ApiResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Pages.Dashboard.ApiResource
{
    /// <summary>
    /// Api作用域
    /// </summary>
    public class ApiScopesModel : PageModel
    {
        private readonly IApiResourceService m_ApiResourceService;
        private readonly IMapper m_Mapper;
        private readonly ILogger<ApiScopesModel> m_Logger;

        /// <summary>
        /// Api资源的Id
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public IdentityServer4.EntityFramework.Entities.ApiResource ApiResEntity { get; set; }

        public ApiScopesModel(IApiResourceService apiResourceService,
            ILogger<ApiScopesModel> logger,
            IMapper mapper)
        {
            this.m_Logger = logger;
            this.m_Mapper = mapper;
            m_ApiResourceService = apiResourceService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            ApiResEntity = await m_ApiResourceService.GetResWithScopes(Id);
            if (ApiResEntity == null)
                return NotFound();

            return Page();
        }
    }
}
