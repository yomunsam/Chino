using AutoMapper;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.SeedData;

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
        }
    }
}
