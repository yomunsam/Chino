using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.ViewModels.Dashboard.ApiResource;
using IdentityServer4.EntityFramework.Entities;

namespace Chino.IdentityServer.Services.ApiResources
{
    public interface IApiResourceService
    {
        Task<ApiResource> AddApiResource(ApiResource apiRes);
        Task DeleteByIdAsync(int Id);
        Task<ApiResource> FindByIdAsync(int Id);
        Task<PaginatedListDto<ApiResource>> GetApiResourcesAsync(int page = 1, int size = 25, string search = null);
        Task<ApiResource> GetResWithScopes(int Id);
        Task<ApiResource> UpdateApiResourceAsync(int Id, ConfigurationViewModel model);
    }
}
