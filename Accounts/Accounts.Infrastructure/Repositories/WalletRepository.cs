using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Constants;
using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;
using Accounts.Domain.Exceptions;
using Accounts.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Accounts.Infrastructure.Repositories
{
    public class WalletRepository : BaseRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(IConfiguration configuration)
            : base(configuration)
        {
            this.TableName = nameof(Wallet) + "s";
        }

        protected override TOutput DataRowToEntity<TOutput>(DataRow dataRow)
        {
            var result = new WalletResponseDto()
            {
                Id = Guid.Parse(dataRow["Id"].ToString()),
                InitialBalance = decimal.Parse(dataRow["InitialBalance"].ToString()),
                CurrentBalance = decimal.Parse(dataRow["CurrentBalance"].ToString()),
                CurrencyCode = (CurrencyCode)int.Parse(dataRow["CurrencyCode"].ToString())
            };

            return (TOutput)Convert.ChangeType(result, typeof(TOutput));
        }

        public async Task DepositSumAsync(Guid accountId, DepositDto deposit)
        {
            await CreateDbIfNotExist();

            if (await GetBalanceAsync(accountId, "InitialBalance") > 0)
            {
                using (var connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlQueryConstants.DEPOSIT_SUM_IF_INITIALBALANCE_ABOVE_ZERO, connection);
                    cmd.Parameters.Add(new SqlParameter("@Id", accountId));
                    cmd.Parameters.Add(new SqlParameter("@Sum", deposit.Sum));
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            
            else
            {
                using (var connection = new SqlConnection(_dbConnectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(SqlQueryConstants.DEPOSIT_SUM_IF_INITIALBALANCE_IS_ZERO, connection);
                    cmd.Parameters.Add(new SqlParameter("@Id", accountId));
                    cmd.Parameters.Add(new SqlParameter("@Sum", deposit.Sum));
                    await cmd.ExecuteNonQueryAsync();
                }
            }           
        }

        public async Task<CurrencyCode> GetCurrencyCodeAsync(Guid accountId)
        {
            await CreateDbIfNotExist();

            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SqlQueryConstants.GET_CURRENCY_CODE, connection);
                cmd.Parameters.Add(new SqlParameter("@Id", accountId));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            int value = int.Parse(dataTable.Rows[0]["test"].ToString());

            return (CurrencyCode)value;
        }

        public async Task<decimal> GetBalanceAsync(Guid accountId, string balanceName)
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SqlQueryConstants.GET_BALANCE, connection);
                cmd.Parameters.Add(new SqlParameter("@Id", accountId));
                cmd.Parameters.Add(new SqlParameter("@BalanceName", balanceName));

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return decimal.Parse(dataTable.Rows[0]["test"].ToString());
        }

        public async Task ChangeCurrencyCodeAsync(Guid accountId, int newCurrency, 
            decimal newInitialBalance, decimal newCurrentBalance)
        {
            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SqlQueryConstants.CHANGE_CURRENCY_CODE, connection);
                cmd.Parameters.Add(new SqlParameter("@Id", accountId));
                cmd.Parameters.Add(new SqlParameter("@NewCurrency", newCurrency));
                cmd.Parameters.Add(new SqlParameter("@NewInitialBalance", newInitialBalance));
                cmd.Parameters.Add(new SqlParameter("@NewCurrentBalance", newCurrentBalance));

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
