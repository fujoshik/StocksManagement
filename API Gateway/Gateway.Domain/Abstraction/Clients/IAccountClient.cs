using Gateway.Domain.DTOs.Authentication;
using Gateway.Domain.DTOs.User;

namespace Gateway.Domain.Abstraction.Clients
{
    public interface IAccountClient
    {
        HttpClient GetApi();
        Task RegisterAsync(RegisterWithSumDTO registerDto);
        Task RegisterTrialAsync(RegisterTrialDTO registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task UpdateUser(Guid id, UserWithoutAccountIdDto user);
    }
}
