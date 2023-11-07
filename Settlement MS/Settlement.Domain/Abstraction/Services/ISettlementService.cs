﻿using Accounts.Domain.DTOs.Wallet;
using Settlement.Domain.DTOs.Settlement;

namespace Settlement.Domain.Abstraction.Services
{
    public interface ISettlementService
    {
        Task<SettlementResponseDto> ExecuteDeal(Guid walletId, decimal price, int amount);
    }
}
