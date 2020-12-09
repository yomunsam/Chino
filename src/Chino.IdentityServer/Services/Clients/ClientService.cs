using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.Utils.PaginatedList;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nekonya;

namespace Chino.IdentityServer.Services.Clients
{
    public class ClientService : IClientService
    {
        private readonly IConfigurationDbContext m_DbContext;
        public ClientService(IConfigurationDbContext dbContext)
        {
            m_DbContext = dbContext;
        }


        public async Task<PaginatedListDto<Client>> GetClientsAsync(int page = 1, int size = 25, string search = null)
        {
            var source = m_DbContext.Clients
                .OrderBy(client => client.Id)
                .AsNoTracking();

            if (!search.IsNullOrEmpty())
                source = source.Where(client => client.ClientName.ToUpper().Contains(search.ToUpper()));

            var result = await PaginatedList<Client>.CreateAsync(source, page, size);
            return result.GetDto();
        }

        public Task<long> GetClientsTotalCount()
        {
            return m_DbContext.Clients.LongCountAsync();
        }

    }
}
