using Accounts.Domain.DTOs.Account;
using AutoMapper;

namespace Accounts.Infrastructure.Mapper
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<RegisterDto, AccountRequestDto>();
        }
    }
}
