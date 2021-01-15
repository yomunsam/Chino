using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chino.IdentityServer.Services.IdentityResources;
using Chino.IdentityServer.ViewModels.Dashboard.IdentityResource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Pages.Dashboard.IdentityResource
{
    /// <summary>
    /// 身份资源总配置页面
    /// </summary>
    public class ConfigurationModel : PageModel
    {
        private readonly IIdentityResouceService m_IdentityResouceService;
        private readonly ILogger<ConfigurationModel> m_Logger;
        private readonly IMapper m_Mapper;

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public IdResConfigurationViewModel ViewModel { get; set; }

        public IdentityServer4.EntityFramework.Entities.IdentityResource IdentityResourceEntity { get; set; }


        public ConfigurationModel(IIdentityResouceService identityResouceService,
            ILogger<ConfigurationModel> logger,
            IMapper mapper)
        {
            this.m_IdentityResouceService = identityResouceService;
            this.m_Logger = logger;
            this.m_Mapper = mapper;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            IdentityResourceEntity = await m_IdentityResouceService.GetAsync(Id);
            if (IdentityResourceEntity == null)
                return NotFound();

            ViewModel = m_Mapper.Map<IdResConfigurationViewModel>(IdentityResourceEntity);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            IdentityResourceEntity = await m_IdentityResouceService.GetAsync(Id);
            if (IdentityResourceEntity == null)
                return NotFound();

            IdentityResourceEntity = m_Mapper.Map(ViewModel, IdentityResourceEntity);
            IdentityResourceEntity = await m_IdentityResouceService.UpdateAsync(IdentityResourceEntity);

            return Page();
        }

    }
}
