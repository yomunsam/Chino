using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chino.Dtos.PaginatedList;
using Chino.Utils.PaginatedList;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Nekonya;

namespace Chino.IdentityServer.Services.ApiResources
{
    public class ApiResourceService : IApiResourceService
    {

        private readonly ConfigurationDbContext m_DbContext;
        private readonly IMapper m_Mapper;

        public ApiResourceService(ConfigurationDbContext dbContext,
            IMapper mapper)
        {
            m_DbContext = dbContext;
            this.m_Mapper = mapper;
        }

        public async Task<PaginatedListDto<ApiResource>> GetApiResourcesAsync(int page = 1, int size = 25, string search = null)
        {
            var source = m_DbContext.ApiResources
                .OrderBy(resources => resources.Id)
                .AsNoTracking();

            if (!search.IsNullOrEmpty())
                source = source.Where(resources => resources.Name.ToUpper().Contains(search.ToUpper()));

            var result = await PaginatedList<ApiResource>.CreateAsync(source, page, size);
            return result.GetDto();
        }

    }
}
