using Authorization.Contracts.Authorization;
using Authorization.Entities.Entities;
using AutoMapper;

namespace Authorization.Core;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserParameters, UserEntity>();

        CreateMap<UserEntity, UserModel>()
            .ForMember(destination => destination.Password, options => options.MapFrom(c => c.PasswordHash));

        CreateMap<UserEntity, AuthenticateResponse>();
    }
}