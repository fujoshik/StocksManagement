﻿using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Account
{
    public class AccountResponseDto : BaseResponseDto
    {
        public string Email { get; set; }
        public Guid WalletId { get; set; }
        public Role Role { get; set; }
        public DateTime? DateToDelete { get; set; }
    }
}
