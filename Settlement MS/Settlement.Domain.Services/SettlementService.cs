using Settlement.Domain.Abstraction.Services;
using Settlement.Domain.DTOs.Settlement;

namespace Settlement.Domain.Services
{
    public class SettlementService : ISettlementService
    {
        private readonly IHttpClientService httpClientService;

        public SettlementService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }
        public async Task<SettlementResponseDto> CheckAccount(string userId, decimal amount)
        {
            SettlementResponseDto response = new SettlementResponseDto();
            
            var userAccountBalance = await httpClientService.GetUserAccountBalance(userId);

            var initialBalance = userAccountBalance.InitialBalance;
            var currentBalance = userAccountBalance.CurrentBalance;

            if(currentBalance > amount)
            {
                var commision = amount * 0.0005M; 
                var newBalance = currentBalance - amount - commision;

                if(newBalance <= 0.85M * initialBalance)
                {
                    response.Success = false;
                    response.Message = "Loss of 15% on initial capital.";
                }
                else
                {
                    response.Success = true;
                    response.Message = "Your account is in good standing.";
                }

                response.InitialBalance = initialBalance;
                response.CurrentBalance = newBalance;

            }
            else
            {
                response.Success = false;
                response.Message = "Insufficient funds.";

                response.InitialBalance = initialBalance;
                response.CurrentBalance = currentBalance;
            }

            return response;
        }
    }
}