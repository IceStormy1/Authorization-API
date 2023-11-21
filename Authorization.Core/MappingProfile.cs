using System;
using Authorization.Contracts.Authorization;
using Authorization.Entities.Entities;
using AutoMapper;

namespace Authorization.Core;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserParameters, UserEntity>()
            .ForMember(destination => destination.DateOfCreate, options => options.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UserEntity, UserModel>()
            .ForMember(destination => destination.Password, options => options.MapFrom(c => c.Password));

        CreateMap<UserEntity, AuthenticateResponse>();
    }
}