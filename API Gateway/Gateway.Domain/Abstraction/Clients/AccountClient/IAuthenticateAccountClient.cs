using Gateway.Domain.DTOs.Authentication;

namespace Gateway.Domain.Abstraction.Clients.AccountClient
{
    public interface IAuthenticateAccountClient
    {
        Task RegisterAsync(RegisterWithSumDTO registerDto);
        Task RegisterTrialAsync(RegisterTrialDTO registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task VerifyCodeAsync(string code);
    }
}
