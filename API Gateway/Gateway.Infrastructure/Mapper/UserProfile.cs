using AutoMapper;
using Gateway.Domain.DTOs.Authentication;
using Gateway.Domain.DTOs.User;

namespace Gateway.Infrastructure.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, UserRequestDto>();
        }
    }
}
