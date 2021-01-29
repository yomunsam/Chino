using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Exceptions.Common;
using Chino.IdentityServer.Services.ApiResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Pages.Dashboard.ApiResource.Scope
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> m_Logger;
        private readonly IApiResourceService m_ApiResourceService;

        /// <summary>
        /// Api Resources Id
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        /// <summary>
        /// Api Resource Scope Id
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int ScopeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IdentityServer4.EntityFramework.Entities.ApiResource ApiResourceEntity { get; set; }

        public IdentityServer4.EntityFramework.Entities.ApiResourceScope ApiResourceScopeEntity { get; set; } 

        public DeleteModel(ILogger<DeleteModel> logger,
            IApiResourceService apiResourceService)
        {
            this.m_Logger = logger;
            this.m_ApiResourceService = apiResourceService;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            ApiResourceEntity = await m_ApiResourceService.GetResWithScopes(Id);
            if (ApiResourceEntity == null)
                return NotFound();

            ApiResourceScopeEntity = ApiResourceEntity.Scopes.Where(rs => rs.Id == ScopeId).FirstOrDefault();
            if (ApiResourceScopeEntity == null)
                return NotFound();

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await m_ApiResourceService.DeleteScopeAsync(this.Id, this.ScopeId);
                return LocalRedirect(this.ReturnUrl ?? "~/");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}
