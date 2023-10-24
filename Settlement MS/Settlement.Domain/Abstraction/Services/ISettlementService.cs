using Settlement.Domain.DTOs.Settlement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.Abstraction.Services
{
    public interface ISettlementService
    {
        Task<SettlementResponseDto> CheckAccount(string userId, decimal amount);
    }
}
