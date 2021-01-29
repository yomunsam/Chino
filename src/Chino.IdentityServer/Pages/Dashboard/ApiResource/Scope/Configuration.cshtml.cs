using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chino.IdentityServer.Services.ApiResources;
using Chino.IdentityServer.Services.ApiScopes;
using Chino.IdentityServer.ViewModels.Dashboard.ApiResource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Pages.Dashboard.ApiResource.Scope
{
    /// <summary>
    /// Api Resources中的ApiScope配置页
    /// </summary>
    public class ConfigurationModel : PageModel
    {
        private readonly IApiResourceService m_ApiResourceService;
        private readonly IApiScopeService m_ApiScopeService;
        private readonly IMapper m_Mapper;
        private readonly ILogger<ConfigurationModel> m_Logger;

        /// <summary>
        /// Api Resource Id
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        /// <summary>
        /// Api Scope Name
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string Scope { get; set; }

        /// <summary>
        /// Api资源实体中的Scope的Id
        /// </summary>
        public int ScopeId { get; set; }


        [BindProperty]
        public CreateUpdateScopeInApiResourceInputModel InputModel { get; set; }

        public IdentityServer4.EntityFramework.Entities.ApiResource ApiResEntity { get; set; }

        public ConfigurationModel(IApiResourceService apiResourceService,
            IApiScopeService apiScopeService,
            ILogger<ConfigurationModel> logger,
            IMapper mapper)
        {
            this.m_Logger = logger;
            this.m_Mapper = mapper;
            m_ApiResourceService = apiResourceService;
            this.m_ApiScopeService = apiScopeService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ApiResEntity = await m_ApiResourceService.GetResWithScopes(Id);
            if (ApiResEntity == null)
                return NotFound();

            var scope = await m_ApiScopeService.FindByName(this.Scope);
            if (scope == null)
                return NotFound();

            ScopeId = ApiResEntity.Scopes.Where(rs => scope.Name.ToUpper() == rs.Scope.ToUpper()).Select(rs => rs.Id).FirstOrDefault();

            InputModel = m_Mapper.Map<CreateUpdateScopeInApiResourceInputModel>(scope);

            return Page();
        }

        /// <summary>
        /// 保存设置
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            ApiResEntity = await m_ApiResourceService.GetResWithScopes(Id);
            if (ApiResEntity == null)
                return NotFound();

            var scope = await m_ApiScopeService.FindByName(this.Scope);
            if (scope == null)
                return NotFound();

            string OriginName = scope.Name;

            scope = m_Mapper.Map(InputModel, scope);

            m_ApiResourceService.DbContext.Attach(scope);
            if(OriginName != InputModel.Name)
            {
                //Scope名字换了，对应的要处理Api资源里的名字
                var resScope = ApiResEntity.Scopes.Where(ars => ars.Scope.ToUpper().Equals(OriginName.ToUpper())).FirstOrDefault();
                if(resScope != null)
                {
                    resScope.Scope = InputModel.Name;
                }

                m_ApiResourceService.DbContext.Attach(ApiResEntity);
            }

            await m_ApiResourceService.DbContext.SaveChangesAsync();

            ScopeId = ApiResEntity.Scopes.Where(rs => scope.Name.ToUpper() == rs.Scope.ToUpper()).Select(rs => rs.Id).FirstOrDefault();

            return Page();
        }

    }
}
