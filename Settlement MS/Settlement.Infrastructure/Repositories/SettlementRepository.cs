using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;
using Dapper;
using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Constants;
using Settlement.Domain.Constants.Queries;
using Settlement.Domain.DTOs.Settlement;
using Settlement.Domain.DTOs.Transaction;
using System.Data.SqlClient;

namespace Settlement.Infrastructure.Repository
{
    public class SettlementRepository : ISettlementRepository
    {
        // just for testing...
        public async Task InsertTransaction(TransactionRequestDto transaction, SettlementResponseDto settlement)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    var random = new Random();
                    var randomTransactionType = random.Next(2) == 0 ? TransactionType.Bought : TransactionType.Sold;

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.InsertTransactionQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@StockTicker", transaction.StockTicker);
                        cmd.Parameters.AddWithValue("@Price", settlement.StockPrice);
                        cmd.Parameters.AddWithValue("@Quantity", transaction.Quantity);
                        cmd.Parameters.AddWithValue("@TransactionType", randomTransactionType);
                        cmd.Parameters.AddWithValue("@AccountId", transaction.AccountId);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task UpdateWalletBalance(Guid Id, decimal newBalance)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using(SqlCommand cmd = new SqlCommand(SqlQueriesConstants.UpdateWalletBalanceQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@NewBalance", newBalance);
                        cmd.Parameters.AddWithValue("@WalletId", Id);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task<List<WalletResponseDto>> GetAllWallets()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    var query = SqlQueriesConstants.GetAllWalletsQuery;
                    var wallets = await connection.QueryAsync<WalletResponseDto>(query);

                    return wallets.ToList();
                }
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task InsertHandledWallets(Guid id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.CheckExistingWalletRecord, connection))
                    {
                        cmd.Parameters.AddWithValue("@WalletId", id);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task<List<Guid>> GetHandledWalletIds()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    var query = SqlQueriesConstants.GetHandledWalletIdsQuery;
                    var walletIds = await connection.QueryAsync<Guid>(query);

                    return walletIds.ToList();
                }
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }
    }
}