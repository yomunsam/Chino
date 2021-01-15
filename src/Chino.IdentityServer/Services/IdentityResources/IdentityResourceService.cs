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

namespace Chino.IdentityServer.Services.IdentityResources
{
    public class IdentityResourceService : IIdentityResouceService
    {
        private readonly ConfigurationDbContext m_DbContext;
        private readonly IMapper m_Mapper;

        public IdentityResourceService(ConfigurationDbContext dbContext,
            IMapper mapper)
        {
            m_DbContext = dbContext;
            this.m_Mapper = mapper;
        }


        public async Task<PaginatedListDto<IdentityResource>> GetIdentityResourcesAsync(int page = 1, int size = 25, string search = null)
        {
            var source = m_DbContext.IdentityResources
                .OrderBy(idr => idr.Id)
                .AsNoTracking();

            if (!search.IsNullOrEmpty())
                source = source.Where(idr => idr.Name.ToUpper().Contains(search.ToUpper())
                    || idr.DisplayName.ToUpper().Contains(search.ToUpper()));

            var result = await PaginatedList<IdentityResource>.CreateAsync(source, page, size);
            return result.GetDto();
        }


        public async Task<IdentityResource> GetAsync(int Id)
        {
            return await m_DbContext.IdentityResources.FindAsync(Id);
        }


        public async Task<IdentityResource> GetWithUserClaimsAsync(int Id)
        {
            var idRes = await m_DbContext.IdentityResources.FindAsync(Id);
            if(idRes != null)
            {
                await m_DbContext.Entry(idRes)
                    .Collection(idr => idr.UserClaims)
                    .LoadAsync();
            }
            return idRes;
        }

        public async Task<IdentityResource> UpdateAsync(IdentityResource identityResource)
        {
            m_DbContext.Attach(identityResource);
            await m_DbContext.SaveChangesAsync();
            return identityResource;
        }

    }
}
