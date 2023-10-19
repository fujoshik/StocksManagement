using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.User;
using AutoMapper;

namespace Accounts.Infrastructure.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<RegisterDto, UserRequestDto>();
        }
    }
}
