using Accounts.Domain.DTOs.Wallet;
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

        public async Task<SettlementResponseDto> ExecuteDeal(Guid walletId, decimal price, int amount)
        {
            SettlementResponseDto response = new SettlementResponseDto();

            var userAccountBalance = await httpClientService.GetWalletBalance(walletId);

            if (userAccountBalance.CurrentBalance >= price * amount)
            {
                decimal commission = (price * amount) * 0.0005M;

                userAccountBalance.CurrentBalance -= (price * amount) + commission;

                if (userAccountBalance.CurrentBalance < 0.85M * userAccountBalance.InitialBalance)
                {
                    response.Success = false;
                    response.Message = "Loss of 15% on initial capital.";
                }
                else
                {
                    response.Success = true;
                    response.Message = "Your account is in good standing.";
                }
            }
            else
            {
                response.Success = false;
                response.Message = "Error while processing the deal.";
            }

            response.InitialBalance = userAccountBalance.InitialBalance;
            response.CurrentBalance = userAccountBalance.CurrentBalance;

            response.TotalBalance = userAccountBalance.CurrentBalance;

            return response;
        }
    }
}