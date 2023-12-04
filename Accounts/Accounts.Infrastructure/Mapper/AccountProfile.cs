using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Authentication;
using Accounts.Domain.DTOs.MongoDB;
using AutoMapper;

namespace Accounts.Infrastructure.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<RegisterTrialDto, AccountRequestDto>();
            CreateMap<RegisterWithSumDto, AccountRequestDto>();
            CreateMap<AccountResponseDto, AccountRequestDto>();
            CreateMap<UserDto, RegisterWithSumDto>().ReverseMap();
            CreateMap<UserDto, RegisterTrialDto>().ReverseMap();
        }
    }
}
