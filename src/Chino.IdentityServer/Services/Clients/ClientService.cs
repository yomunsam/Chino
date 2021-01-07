using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.Exceptions.Common;
using Chino.IdentityServer.ViewModels.Dashboard.Client;
using Chino.Utils.PaginatedList;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Nekonya;

namespace Chino.IdentityServer.Services.Clients
{
    public class ClientService : IClientService
    {
        private readonly ConfigurationDbContext m_DbContext;
        private readonly IMapper m_Mapper;

        public ClientService(ConfigurationDbContext dbContext,
            IMapper mapper)
        {
            m_DbContext = dbContext;
            this.m_Mapper = mapper;
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

        public async Task<Client> GetClientAsync(int Id)
        {
            return await m_DbContext.Clients.FindAsync(Id);
        }

        public Task<long> GetClientsTotalCount()
        {
            return m_DbContext.Clients.LongCountAsync();
        }

        public async Task<Client> UpdateClient(int ClientId, ConfigurationClientViewModel viewModel)
        {
            var clientEntity = await m_DbContext.Clients.FindAsync(ClientId);
            if (clientEntity == null)
                throw new NotFoundException();


            clientEntity = m_Mapper.Map(viewModel, clientEntity);


            await m_DbContext.SaveChangesAsync();

            return clientEntity;
        }


        public async Task<Client> CreateClient(string clientId, string clientName, string desc)
        {
            if (await m_DbContext.Clients.AnyAsync(client => client.ClientId.Equals(clientId)))
                throw new AlreadyExists();

            var clientEntity = new Client();
            clientEntity.ClientId = clientId;
            clientEntity.ClientName = clientName;
            clientEntity.Description = desc;


            await m_DbContext.Clients.AddAsync(clientEntity);
            await m_DbContext.SaveChangesAsync();
            return clientEntity;
        }

    }
}
