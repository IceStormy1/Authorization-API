using Authorization.Contracts.Authorization;
using Authorization.Entities.Entities;
using AutoMapper;

namespace Authorization
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserModel, UserEntity>().ReverseMap();
        }
    }
}
