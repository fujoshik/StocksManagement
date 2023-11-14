using Accounts.Domain.DTOs.Authentication;
using Accounts.Domain.DTOs.Wallet;
using AutoMapper;

namespace Accounts.Infrastructure.Mapper
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<RegisterTrialDto, WalletRequestDto>();

            CreateMap<RegisterWithSumDto, WalletRequestDto>()
                .ForMember(dest => dest.InitialBalance, opt => opt.MapFrom(x => x.Sum))
                .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(x => x.Sum));
        }
    }
}
