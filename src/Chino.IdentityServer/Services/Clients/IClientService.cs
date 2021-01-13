using System.Threading.Tasks;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.ViewModels.Dashboard.Client;
using IdentityServer4.EntityFramework.Entities;

namespace Chino.IdentityServer.Services.Clients
{
    public interface IClientService
    {
        Task<Client> CreateClient(string clientId, string clientName, string desc);
        Task<Client> DeleteClientById(int Id);

        /// <summary>
        /// 获取包含了“允许跨域来源”信息的客户端信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<Client> GetClientAndAllowedCorsOrigins(int clientId);

        /// <summary>
        /// 获取包含了“允许的授权类型”信息的客户端信息
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<Client> GetClientAndAllowedGrantTypes(int clientId);
        Task<Client> GetClientAsync(int Id);
        Task<PaginatedListDto<Client>> GetClientsAsync(int page = 1, int size = 25, string search = null);
        Task<long> GetClientsTotalCount();
        Task<Client> Update(Client client);
        Task<Client> UpdateClient(int ClientId, ConfigurationClientViewModel viewModel);
    }
}
