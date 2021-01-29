using AutoMapper;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Dtos.Account;
using Chino.IdentityServer.SeedData;
using Chino.IdentityServer.ViewModels.Dashboard.ApiResource;
using Chino.IdentityServer.ViewModels.Dashboard.Client;
using Chino.IdentityServer.ViewModels.Dashboard.IdentityResource;

namespace Chino.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<SeedDataJsonConfig.User, ChinoUser>();
            //CreateMap<Meow, MeowDto>();
            //......

            CreateMap<ChinoAccountConfiguration, ChinoAccountConfiguration>();

            CreateMap<ChinoUser, UserInfoDto>();

            CreateMap<IdentityServer4.EntityFramework.Entities.Client, ConfigurationClientViewModel>()
                .ReverseMap();

            CreateMap<CreateApiResourceInputModel, IdentityServer4.EntityFramework.Entities.ApiResource>();
            CreateMap<ConfigurationViewModel, IdentityServer4.EntityFramework.Entities.ApiResource>()
                .ReverseMap();

            CreateMap<IdResConfigurationViewModel, IdentityServer4.EntityFramework.Entities.IdentityResource>()
                .ReverseMap();

            CreateMap<CreateUpdateScopeInApiResourceInputModel, IdentityServer4.EntityFramework.Entities.ApiScope>()
                .ReverseMap();
        }
    }
}
