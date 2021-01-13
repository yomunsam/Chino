using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.ViewModels.Dashboard.Client;
using IdentityServer4.EntityFramework.Entities;

namespace Chino.IdentityServer.Services.Clients
{
    public interface IClientService
    {
        Task<Client> CreateClient(string clientId, string clientName, string desc);
        Task DeleteClientById(int Id);
        Task<Client> GetClientAsync(int Id);
        Task<PaginatedListDto<Client>> GetClientsAsync(int page = 1, int size = 25, string search = null);
        Task<long> GetClientsTotalCount();
        Task<Client> UpdateClient(int ClientId, ConfigurationClientViewModel viewModel);
    }
}
