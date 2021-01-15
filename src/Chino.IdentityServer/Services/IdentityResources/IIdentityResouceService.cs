using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using IdentityServer4.EntityFramework.Entities;

namespace Chino.IdentityServer.Services.IdentityResources
{
    public interface IIdentityResouceService
    {
        Task<IdentityResource> GetAsync(int Id);
        Task<PaginatedListDto<IdentityResource>> GetIdentityResourcesAsync(int page = 1, int size = 25, string search = null);
        Task<IdentityResource> GetWithUserClaimsAsync(int Id);
        Task<IdentityResource> UpdateAsync(IdentityResource identityResource);
    }
}
