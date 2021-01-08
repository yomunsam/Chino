using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chino.IdentityServer.Exceptions.Common;
using Chino.IdentityServer.Services.ApiResources;
using Chino.IdentityServer.Services.Clients;
using Chino.IdentityServer.ViewModels.Dashboard.ApiResource;
using Chino.IdentityServer.ViewModels.Dashboard.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Chino.IdentityServer.Pages.Dashboard.ApiResource
{
    /// <summary>
    /// ´´½¨ApiResouceµÄPageModel
    /// </summary>
    public class CreateModel : PageModel
    {
        private readonly IApiResourceService m_ApiResourceService;
        private readonly IStringLocalizer<CreateModel> L;
        private readonly IMapper m_Mapper;

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public CreateApiResourceInputModel InputModel { get; set; }


        public CreateModel(IApiResourceService apiResourceService,
            IStringLocalizer<CreateModel> localizer,
            IMapper mapper)
        {
            this.m_ApiResourceService = apiResourceService;
            this.L = localizer;
            this.m_Mapper = mapper;
        }

        public void OnGet()
        {
            this.InputModel = new CreateApiResourceInputModel();
            this.InputModel.Enabled = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var apiResources = m_Mapper.Map<IdentityServer4.EntityFramework.Entities.ApiResource>(InputModel);
                var client = await m_ApiResourceService.AddApiResource(apiResources);
                return LocalRedirect(this.ReturnUrl ?? "~/");
            }
            catch (AlreadyExistsException)
            {
                this.ModelState.AddModelError(string.Empty, L["name_already_exists", InputModel.Name]);
                return Page();
            }
        }
    }
}
