﻿using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.DTOs.Transaction;
using Accounts.Domain.Enums;
using Accounts.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Accounts.Infrastructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IConfiguration configuration)
            : base(configuration)
        {
            this.TableName = nameof(Transaction) + "s";
        }

        protected override TOutput DataRowToEntity<TOutput>(DataRow dataRow)
        {
            var result = new TransactionResponseDto()
            {
                Id = Guid.Parse(dataRow["Id"].ToString()),
                StockTicker = dataRow["StockTicker"].ToString(),
                TransactionType = (TransactionType)int.Parse(dataRow["TransactionType"].ToString()),
                Price = decimal.Parse(dataRow["Price"].ToString()),
                Quantity = int.Parse(dataRow["Quantity"].ToString()),
                DateOfTransaction = DateTime.Parse(dataRow["DateOfTransaction"].ToString()),
                AccountId = Guid.Parse(dataRow["AccountId"].ToString())
            };

            return (TOutput)Convert.ChangeType(result, typeof(TOutput));
        }
    }
}