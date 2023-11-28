using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;
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
        public async Task InsertTransaction(TransactionRequestDto transaction, SettlementResponseDto settlement)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();
                    await CreateFailedTransactionsTable();
                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.InsertTransactionQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@StockTicker", transaction.StockTicker);
                        cmd.Parameters.AddWithValue("@Price", transaction.StockPrice);
                        cmd.Parameters.AddWithValue("@Quantity", transaction.Quantity);
                        cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                        cmd.Parameters.AddWithValue("@AccountId", transaction.AccountId);

                        if(!settlement.Success)
                        {
                            await InsertIntoFailedTransaction(transaction);
                        }
                        else
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task InsertIntoFailedTransaction(TransactionRequestDto transaction)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.InsertIntoFailedTransaction, connection))
                    {
                        cmd.Parameters.AddWithValue(@"WalletId", transaction.WalletId);
                        cmd.Parameters.AddWithValue("@StockTicker", transaction.StockTicker);
                        cmd.Parameters.AddWithValue("@Price", transaction.StockPrice);
                        cmd.Parameters.AddWithValue("@Quantity", transaction.Quantity);
                        cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
                        cmd.Parameters.AddWithValue("@AccountId", transaction.AccountId);
                        cmd.Parameters.AddWithValue("@Date", transaction.Date);

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

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.UpdateWalletBalanceQuery, connection))
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
                List<WalletResponseDto> wallets = new List<WalletResponseDto>();

                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.GetAllWalletsQuery, connection))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                WalletResponseDto wallet = new WalletResponseDto();
                                wallet.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                                wallet.InitialBalance = reader.GetDecimal(reader.GetOrdinal("InitialBalance"));
                                wallet.CurrentBalance = reader.GetDecimal(reader.GetOrdinal("CurrentBalance"));
                                wallet.CurrencyCode = (CurrencyCode)reader.GetInt32(reader.GetOrdinal("CurrencyCode"));
                                wallets.Add(wallet);
                            }
                        }
                    }
                }

                return wallets;
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task<List<TransactionRequestDto>> GetFailedTransactions()
        {
            try
            {
                List<TransactionRequestDto> transactions = new List<TransactionRequestDto>();

                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.GetAllFailedTransactions, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                TransactionRequestDto transaction = new TransactionRequestDto();
                                transaction.WalletId = reader.GetGuid(reader.GetOrdinal("WalletId"));
                                transaction.StockTicker = reader.GetString(reader.GetOrdinal("StockTicker"));
                                transaction.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                                transaction.AccountId = reader.GetGuid(reader.GetOrdinal("AccountId"));
                                transaction.TransactionType = (Domain.Enums.TransactionType)(TransactionType)reader.GetInt32(reader.GetOrdinal("TransactionType"));
                                transaction.StockPrice = reader.GetDecimal(reader.GetOrdinal("Price"));
                                transaction.Date = reader.GetString(reader.GetOrdinal("Date"));
                                transactions.Add(transaction);
                            }
                        }
                    }
                }
                return transactions;
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
                List<Guid> walletIds = new List<Guid>();

                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.GetHandledWalletIdsQuery, connection))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Guid walletId = reader.GetGuid(reader.GetOrdinal("WalletId"));
                                walletIds.Add(walletId);
                            }
                        }
                    }
                }

                return walletIds;
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task CreateFailedTransactionsTable()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.CreateTableTransactionFailed, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task DeleteFailedTransaction(Guid walletId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.DeleteFailedTransaction, connection))
                    {
                        cmd.Parameters.AddWithValue("@WalletId", walletId);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }
    }
}