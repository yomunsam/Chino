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
                source = source.Where(client => client.ClientName.ToUpper().Contains(search.ToUpper())
                    || client.ClientId.ToUpper().Contains(search.ToUpper()));

            var result = await PaginatedList<Client>.CreateAsync(source, page, size);
            return result.GetDto();
        }

        public async Task<Client> GetClientAsync(int Id)
        {
            return await m_DbContext.Clients.FindAsync(Id);
        }


        public async Task<Client> GetClientAndAllowedCorsOrigins(int clientId)
        {
            var client = await m_DbContext.Clients.FindAsync(clientId);
            if(client != null)
            {
                await m_DbContext.Entry(client)
                    .Collection(c => c.AllowedCorsOrigins)
                    .LoadAsync();
            }

            return client;
        }

        public async Task<Client> GetClientAndAllowedScopes(int clientId)
        {
            var client = await m_DbContext.Clients.FindAsync(clientId);
            if (client != null)
            {
                await m_DbContext.Entry(client)
                    .Collection(c => c.AllowedScopes)
                    .LoadAsync();
            }

            return client;
        }

        public async Task<Client> GetClientAndRedirectUris(int clientId)
        {
            var client = await m_DbContext.Clients.FindAsync(clientId);
            if (client != null)
            {
                await m_DbContext.Entry(client)
                    .Collection(c => c.RedirectUris)
                    .LoadAsync();
            }

            return client;
        }

        public async Task<Client> GetClientAndAllowedGrantTypes(int clientId)
        {
            var client = await m_DbContext.Clients.FindAsync(clientId);
            if (client != null)
            {
                await m_DbContext.Entry(client)
                    .Collection(c => c.AllowedGrantTypes)
                    .LoadAsync();
            }

            return client;
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

        public async Task<Client> Update(Client client)
        {
            m_DbContext.Attach(client);
            await m_DbContext.SaveChangesAsync();
            return client;
        }

        public async Task<Client> DeleteClientById(int Id)
        {
            var client = await m_DbContext.Clients.FindAsync(Id);
            if (client == null)
                throw new NotFoundException();

            m_DbContext.Clients.Remove(client);
            await m_DbContext.SaveChangesAsync();
            return client;
        }


        public async Task<Client> CreateClient(string clientId, string clientName, string desc)
        {
            if (await m_DbContext.Clients.AnyAsync(client => client.ClientId.Equals(clientId)))
                throw new AlreadyExistsException();

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
