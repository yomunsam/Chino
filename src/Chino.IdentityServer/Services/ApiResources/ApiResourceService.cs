using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chino.Dtos.PaginatedList;
using Chino.IdentityServer.Exceptions.Common;
using Chino.IdentityServer.ViewModels.Dashboard.ApiResource;
using Chino.Utils.PaginatedList;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
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

        public ConfigurationDbContext DbContext => m_DbContext;

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

        public async Task<ApiResource> FindByIdAsync(int Id)
        {
            return await m_DbContext.ApiResources.FindAsync(Id);
        }

        public async Task DeleteAsync(int Id)
        {
            var apiRes = await m_DbContext.ApiResources.FindAsync(Id);
            if (apiRes == null)
                throw new NotFoundException();

            //删除与该Api资源相关的作用域
            await m_DbContext.Entry(apiRes)
                    .Collection(res => res.Scopes)
                    .LoadAsync();
            var scopeNames = apiRes.Scopes.Select(rs => rs.Scope.ToUpper()).ToList();
            var scopes = await m_DbContext.ApiScopes.Where(sc => scopeNames.Contains(sc.Name.ToUpper()))
                .ToListAsync();
            m_DbContext.ApiScopes.RemoveRange(scopes);

            m_DbContext.ApiResources.Remove(apiRes);
            await m_DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 添加Api资源
        /// </summary>
        /// <param name="apiRes"></param>
        /// <returns></returns>
        public async Task<ApiResource> AddApiResource(ApiResource apiRes)
        {
            if(await m_DbContext.ApiResources.AnyAsync(api => api.Name == apiRes.Name))
            {
                throw new AlreadyExistsException("Name");
            }

            await m_DbContext.ApiResources.AddAsync(apiRes);
            await m_DbContext.SaveChangesAsync();
            return apiRes;
        }

        public async Task<ApiResource> UpdateApiResourceAsync(int Id, ConfigurationViewModel model)
        {
            var apiResEntity = await m_DbContext.ApiResources.FindAsync(Id);
            if (apiResEntity == null)
                throw new NotFoundException();

            apiResEntity = m_Mapper.Map(model, apiResEntity);

            await m_DbContext.SaveChangesAsync();
            return apiResEntity;
        }

        /// <summary>
        /// 获取加载了Scopes关联数据的Api资源对象
        /// </summary>
        /// <param name="apiResId"></param>
        /// <returns></returns>
        public async Task<ApiResource> GetResWithScopes(int Id)
        {
            var apiResEntity = await m_DbContext.ApiResources.FindAsync(Id);
            if(apiResEntity != null)
            {
                await m_DbContext.Entry(apiResEntity)
                    .Collection(res => res.Scopes)
                    .LoadAsync();
            }


            return apiResEntity;
        }

        /// <summary>
        /// 在Api资源中添加Api作用域
        /// </summary>
        /// <param name="Id">Api资源Id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ApiResource> AddApiScope(int Id, CreateUpdateScopeInApiResourceInputModel input)
        {
            var apiResEntity = await m_DbContext.ApiResources.FindAsync(Id);
            if (apiResEntity != null)
            {
                await m_DbContext.Entry(apiResEntity)
                    .Collection(res => res.Scopes)
                    .LoadAsync();
            }

            if (apiResEntity == null)
                throw new NotFoundException();

            //检查ScopeName是否已存在
            if(await m_DbContext.ApiScopes.AnyAsync(scope => scope.Name.ToUpper().Equals(input.Name.ToUpper())))
            {
                throw new AlreadyExistsException();
            }

            //执行添加
            var scopeEntity = m_Mapper.Map<ApiScope>(input);
            await m_DbContext.ApiScopes.AddAsync(scopeEntity);
            apiResEntity.Scopes.Add(new ApiResourceScope
            {
                Scope = input.Name
            });

            await m_DbContext.SaveChangesAsync();
            return apiResEntity;
        }

        /// <summary>
        /// 删除作用域
        /// </summary>
        /// <param name="Id">Api资源 ID</param>
        /// <param name="ScopeId">Api 资源 作用域 Id</param>
        /// <returns></returns>
        public async Task DeleteScopeAsync(int Id, int ScopeId)
        {
            var apiResEntity = await m_DbContext.ApiResources.FindAsync(Id);
            if (apiResEntity == null)
                throw new NotFoundException();

            await m_DbContext.Entry(apiResEntity)
                    .Collection(res => res.Scopes)
                    .LoadAsync();

            var resScope = apiResEntity.Scopes.Where(rs => rs.Id == ScopeId).FirstOrDefault();
            if (resScope != null)
                apiResEntity.Scopes.Remove(resScope);

            //删除Api Scope
            var scope = m_DbContext.ApiScopes.Where(s => s.Name.ToUpper().Equals(resScope.Scope.ToUpper())).FirstOrDefault();
            if(scope != null)
            {
                m_DbContext.ApiScopes.Remove(scope);
            }

            await m_DbContext.SaveChangesAsync();
        }
        
    }
}
