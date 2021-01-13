using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.IdentityServer.Exceptions.Common;
using Chino.IdentityServer.Services.ApiResources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Pages.Dashboard.ApiResource
{
    public class DeleteModel : PageModel
    {

        private readonly ILogger<DeleteModel> m_Logger;
        private readonly IApiResourceService m_ApiResourceService;

        public DeleteModel(ILogger<DeleteModel> logger,
            IApiResourceService apiResourceService)
        {
            this.m_Logger = logger;
            this.m_ApiResourceService = apiResourceService;
        }


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IdentityServer4.EntityFramework.Entities.ApiResource ApiResourceEntity { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            ApiResourceEntity = await m_ApiResourceService.FindByIdAsync(Id);
            if (ApiResourceEntity == null)
                return NotFound();

            return Page();
        }

        public async Task <IActionResult> OnPostAsync()
        {
            try
            {
                await m_ApiResourceService.DeleteByIdAsync(this.Id);
                return LocalRedirect(this.ReturnUrl ?? "~/");
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

    }
}
