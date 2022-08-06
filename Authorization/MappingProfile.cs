using Authorization.Contracts.Authorization;
using Authorization.Entities.Entities;
using AutoMapper;
using System;

namespace Authorization
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserParameters, UserEntity>()
                .ForMember(destination => destination.DateOfCreate,
                    options => options.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UserEntity, UserModel>();
            CreateMap<UserEntity, AuthenticateResponse>();
        }
    }
}
