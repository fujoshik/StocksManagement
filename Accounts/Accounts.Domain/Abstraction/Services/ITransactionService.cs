﻿using Accounts.Domain.DTOs.Transaction;

namespace Accounts.Domain.Abstraction.Services
{
    public interface ITransactionService
    {
        Task CreateAsync(TransactionRequestDto transaction);
    }
}
