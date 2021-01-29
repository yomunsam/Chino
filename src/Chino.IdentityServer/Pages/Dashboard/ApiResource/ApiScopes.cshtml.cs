using System.Threading.Tasks;
using AutoMapper;
using Chino.IdentityServer.Exceptions.Common;
using Chino.IdentityServer.Services.ApiResources;
using Chino.IdentityServer.ViewModels.Dashboard.ApiResource;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Chino.IdentityServer.Pages.Dashboard.ApiResource
{
    /// <summary>
    /// Api资源下的Api作用域
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

        [BindProperty]
        public CreateUpdateScopeInApiResourceInputModel CreateInputModel { get; set; }

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

            //下面的操作，是把实体上的一些默认值给InputModel
            var apiScopeEntity = new IdentityServer4.EntityFramework.Entities.ApiScope();
            CreateInputModel = m_Mapper.Map<CreateUpdateScopeInApiResourceInputModel>(apiScopeEntity);

            return Page();
        }

        /// <summary>
        /// 在此创建作用域
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ApiResEntity = await m_ApiResourceService.AddApiScope(this.Id, this.CreateInputModel);
                return Page();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (AlreadyExistsException)
            {
                ApiResEntity = await m_ApiResourceService.GetResWithScopes(Id);
                ModelState.AddModelError(string.Empty, $"Api Scope \"{this.CreateInputModel?.Name}\" already exists.");
                return Page();
            }
        }

    }
}
