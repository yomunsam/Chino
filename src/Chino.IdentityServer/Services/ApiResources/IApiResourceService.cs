using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.ViewModels.Dashboard.ApiResource;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;

namespace Chino.IdentityServer.Services.ApiResources
{
    public interface IApiResourceService
    {
        ConfigurationDbContext DbContext { get; }

        Task<ApiResource> AddApiResource(ApiResource apiRes);

        /// <summary>
        /// 在Api资源中添加Api作用域
        /// </summary>
        /// <param name="Id">Api资源的Id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ApiResource> AddApiScope(int Id, CreateUpdateScopeInApiResourceInputModel input);
        Task DeleteAsync(int Id);
        Task DeleteScopeAsync(int Id, int ScopeId);
        Task<ApiResource> FindByIdAsync(int Id);
        Task<PaginatedListDto<ApiResource>> GetApiResourcesAsync(int page = 1, int size = 25, string search = null);
        Task<ApiResource> GetResWithScopes(int Id);
        Task<ApiResource> UpdateApiResourceAsync(int Id, ConfigurationViewModel model);
    }
}
