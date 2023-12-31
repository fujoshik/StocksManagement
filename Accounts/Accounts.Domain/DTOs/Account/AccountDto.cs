﻿using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Account
{
    public class AccountDto : BaseResponseDto
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Role Role { get; set; }
        public DateTime? DateToDelete { get; set; }
    }
}
