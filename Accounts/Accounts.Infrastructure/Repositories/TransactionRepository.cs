using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Constants;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Transaction;
using Accounts.Domain.Enums;
using Accounts.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

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

        public async Task DeleteByAccountIdAsync(Guid accountId)
        {
            await CreateDbIfNotExist();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SqlQueryConstants.DELETE_BY_ACCOUNTID_FROM_TRANSACTIONS, connection);
                cmd.Parameters.Add(new SqlParameter("@AccountId", accountId));
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<TransactionResponseDto>> GetSoldTransactionsByAccountId(Guid accountId)
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SqlQueryConstants.GET_SOLD_TRANSACTIONS_BY_ACCOUNTID, connection);
                cmd.Parameters.Add(new SqlParameter("@AccountId", accountId));
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable
                .AsEnumerable()
                .Select(x => new TransactionResponseDto
                {
                    Id = Guid.Parse(x["Id"].ToString()),
                    StockTicker = x["StockTicker"].ToString(),
                    TransactionType = (TransactionType)int.Parse(x["TransactionType"].ToString()),
                    Price = decimal.Parse(x["Price"].ToString()),
                    Quantity = int.Parse(x["Quantity"].ToString()),
                    DateOfTransaction = DateTime.Parse(x["DateOfTransaction"].ToString()),
                    AccountId = Guid.Parse(x["AccountId"].ToString())
                })
                .ToList();
        }

        public async Task<List<TransactionResponseDto>> GetTransactionsByAccountIdAndTickerAsync(Guid accountId, string ticker)
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SqlQueryConstants.GET_TRANSACTIONS_BY_ACCOUNTID_AND_TICKER, connection);
                cmd.Parameters.Add(new SqlParameter("@AccountId", accountId));
                cmd.Parameters.Add(new SqlParameter("@Ticker", ticker));
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable
                .AsEnumerable()
                .Select(x => new TransactionResponseDto
                {
                    Id = Guid.Parse(x["Id"].ToString()),
                    StockTicker = x["StockTicker"].ToString(),
                    TransactionType = (TransactionType)int.Parse(x["TransactionType"].ToString()),
                    Price = decimal.Parse(x["Price"].ToString()),
                    Quantity = int.Parse(x["Quantity"].ToString()),
                    DateOfTransaction = DateTime.Parse(x["DateOfTransaction"].ToString()),
                    AccountId = Guid.Parse(x["AccountId"].ToString())
                })
                .ToList();
        }
    }
}
