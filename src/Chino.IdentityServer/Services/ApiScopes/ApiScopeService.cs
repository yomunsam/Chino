using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chino.Dtos.PaginatedList;
using Chino.Utils.PaginatedList;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Nekonya;

namespace Chino.IdentityServer.Services.ApiScopes
{
    public class ApiScopeService : IApiScopeService
    {
        private readonly ConfigurationDbContext m_DbContext;
        private readonly IMapper m_Mapper;

        public ApiScopeService(ConfigurationDbContext dbContext,
            IMapper mapper)
        {
            this.m_DbContext = dbContext;
            this.m_Mapper = mapper;
        }

        public async Task<PaginatedListDto<ApiScope>> GetListAsync(int page = 1, int size = 25, string search = null)
        {
            var source = m_DbContext.ApiScopes
                .OrderBy(resources => resources.Id)
                .AsNoTracking();

            if (!search.IsNullOrEmpty())
                source = source.Where(resources => resources.Name.ToUpper().Contains(search.ToUpper()));

            var result = await PaginatedList<ApiScope>.CreateAsync(source, page, size);
            return result.GetDto();
        }

        public async Task<ApiScope> GetAsync(int Id)
        {
            return await m_DbContext.ApiScopes.FindAsync(Id);
        }


    }
}
