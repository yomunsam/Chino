using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using IdentityServer4.EntityFramework.Entities;

namespace Chino.IdentityServer.Services.ApiResources
{
    public interface IApiResourceService
    {
        Task<PaginatedListDto<ApiResource>> GetApiResourcesAsync(int page = 1, int size = 25, string search = null);
    }
}
