﻿using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Authentication;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IAccountService
    {
        Task<AccountResponseDto> CreateAsync(RegisterDto registerDto);
        Task<AccountResponseDto> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Guid GetLoggedAccount();
    }
}
