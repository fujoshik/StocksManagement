using Settlement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.DTOs.FailedTransaction
{
    public class FailedTransactionDto : BaseDto
    {
        public Guid TransactionId { get; set; }
    }
}
