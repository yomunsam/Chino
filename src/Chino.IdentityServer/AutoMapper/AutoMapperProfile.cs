﻿using AutoMapper;
using Chino.EntityFramework.Shared.Entities.User;
using Chino.IdentityServer.Configures;
using Chino.IdentityServer.Dtos.Account;
using Chino.IdentityServer.SeedData;
using Chino.IdentityServer.ViewModels.Dashboard.Client;

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
        }
    }
}
