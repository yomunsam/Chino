using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using IdentityServer4.EntityFramework.Entities;

namespace Chino.IdentityServer.Services.Clients
{
    public interface IClientService
    {
        Task<PaginatedListDto<Client>> GetClientsAsync(int page = 1, int size = 25, string search = null);
        Task<long> GetClientsTotalCount();
    }
}
