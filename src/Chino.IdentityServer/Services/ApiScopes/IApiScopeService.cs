using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using IdentityServer4.EntityFramework.Entities;

namespace Chino.IdentityServer.Services.ApiScopes
{
    public interface IApiScopeService
    {
        Task<ApiScope> GetAsync(int Id);
        Task<PaginatedListDto<ApiScope>> GetListAsync(int page = 1, int size = 25, string search = null);
    }
}
