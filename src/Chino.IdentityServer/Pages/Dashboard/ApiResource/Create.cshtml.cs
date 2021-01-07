using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public CreateApiResourceInputModel InputModel { get; set; }


        public CreateModel(IApiResourceService apiResourceService,
            IStringLocalizer<CreateModel> localizer)
        {
            this.m_ApiResourceService = apiResourceService;
            this.L = localizer;
        }

        public void OnGet()
        {
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    try
        //    {
        //        var client = await m_ApiResourceService.CreateClient(ViewModel.ClientId, ViewModel.ClientName, ViewModel.Description);
        //        return LocalRedirect(this.ReturnUrl ?? "~/");
        //    }
        //    catch(AlreadyExists)
        //    {
        //        this.ModelState.AddModelError(string.Empty, L["id_already_exists", ViewModel.ClientId]);
        //        return Page();
        //    }
        //}
    }
}
