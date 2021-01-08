using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chino.IdentityServer.Exceptions.Common;
using Chino.IdentityServer.Services.ApiResources;
using Chino.IdentityServer.ViewModels.Dashboard.ApiResource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chino.IdentityServer.Pages.Dashboard.ApiResource
{
    public class ConfigurationModel : PageModel
    {
        private readonly IApiResourceService m_ApiResource;
        private readonly IMapper m_Mapper;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public ConfigurationViewModel ViewModel { get; set; }

        public IdentityServer4.EntityFramework.Entities.ApiResource ApiResourceEntity { get; set; }

        public ConfigurationModel(IApiResourceService apiResource,
            IMapper mapper)
        {
            this.m_ApiResource = apiResource;
            this.m_Mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ApiResourceEntity = await m_ApiResource.FindApiResourceByIdAsync(this.Id);
            if (ApiResourceEntity == null)
                return NotFound();

            this.ViewModel = m_Mapper.Map<ConfigurationViewModel>(ApiResourceEntity);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ApiResourceEntity = await m_ApiResource.UpdateApiResourceAsync(this.Id, this.ViewModel);
                this.ViewModel = m_Mapper.Map<ConfigurationViewModel>(ApiResourceEntity);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }


            return Page();
        }

    }
}
