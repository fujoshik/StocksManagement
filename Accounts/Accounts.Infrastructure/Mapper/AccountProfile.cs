using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Authentication;
using AutoMapper;

namespace Accounts.Infrastructure.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<RegisterTrialDto, AccountRequestDto>();
            CreateMap<RegisterWithSumDto, AccountRequestDto>();
        }
    }
}
