using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;
using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Constants;
using Settlement.Domain.Constants.Queries;
using Settlement.Domain.DTOs.Handled;
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
                if(transaction.Id == Guid.Empty)
                {
                    transaction.Id = Guid.NewGuid();
                }

                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.InsertTransactionQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", transaction.Id);
                        cmd.Parameters.AddWithValue("@StockTicker", transaction.StockTicker);
                        cmd.Parameters.AddWithValue("@Price", transaction.StockPrice);
                        cmd.Parameters.AddWithValue("@Quantity", transaction.Quantity);
                        cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
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

        public async Task InsertIntoFailedTransaction(TransactionRequestDto transaction)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();
                    await CreateFailedTransactionsTable();

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

        public async Task<List<TransactionRequestDto>> GetFailedTransactions()
        {
            try
            {
                List<TransactionRequestDto> transactions = new List<TransactionRequestDto>();

                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using(SqlCommand cmd = new SqlCommand(SqlQueriesConstants.GetAllFailedTransactions, connection))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(await reader.ReadAsync())
                            {
                                TransactionRequestDto transaction = new TransactionRequestDto();
                                transaction.WalletId = reader.GetGuid(reader.GetOrdinal("WalletId"));
                                transaction.StockTicker = reader.GetString(reader.GetOrdinal("StockTicker"));
                                transaction.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                                transaction.AccountId = reader.GetGuid(reader.GetOrdinal("AccountId"));
                                transaction.TransactionType = (Domain.Enums.TransactionType)reader.GetInt32(reader.GetOrdinal("TransactionType"));
                                transaction.StockPrice = reader.GetDecimal(reader.GetOrdinal("Price"));
                                transaction.Date = reader.GetString(reader.GetOrdinal("Date"));
                                transactions.Add(transaction);
                            }
                        }
                    }
                }
                return transactions;
            }
            catch(SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task InsertHandledWallets(Guid walletId, Guid accountId, Guid transactionId)
        {
            try
            {
                await CreateHandledWalletsTable();

                using(SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using(SqlCommand cmd = new SqlCommand(SqlQueriesConstants.InsertIntoHandledWallets, connection))
                    {
                        cmd.Parameters.AddWithValue("@WalletId", walletId);
                        cmd.Parameters.AddWithValue("@AccountId", accountId);
                        cmd.Parameters.AddWithValue("@TransactionId", transactionId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch(SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task<List<HandledWalletsDto>> GetHandledWalletIds()
        {
            try
            {
                List<HandledWalletsDto> walletIds = new List<HandledWalletsDto>();

                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.GetHandledWalletIdsQuery, connection))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                HandledWalletsDto handledWallets = new HandledWalletsDto();
                                handledWallets.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                                handledWallets.WalletId = reader.GetGuid(reader.GetOrdinal("WalletId"));
                                handledWallets.AccountId = reader.GetGuid(reader.GetOrdinal("AccountId"));
                                handledWallets.TransactionId = reader.GetGuid(reader.GetOrdinal("TransactionId"));
                                walletIds.Add(handledWallets);
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
                using(SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using(SqlCommand cmd = new SqlCommand(SqlQueriesConstants.CreateTableTransactionFailed, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch(SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task DeleteFailedTransaction(Guid walletId)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using(SqlCommand cmd = new SqlCommand(SqlQueriesConstants.DeleteFailedTransaction, connection))
                    {
                        cmd.Parameters.AddWithValue("@WalletId", walletId);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch(SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task<bool> CheckValidWalletId(Guid walletId)
        {
            try
            {
                bool isValid = false;

                using(SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using(SqlCommand cmd = new SqlCommand(SqlQueriesConstants.CheckValidWalletId, connection))
                    {
                        cmd.Parameters.AddWithValue("@WalletId", walletId);
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            isValid = reader.HasRows;
                        }
                    }
                }

                return isValid;
            }
            catch(SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task CreateHandledWalletsTable()
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.CreateHandledWalletsTable, connection))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch(SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task<WalletResponseDto> GetWalletById(Guid walletId)
        {
            try
            {
                WalletResponseDto wallet = null;

                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.GetWalletByIdQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@WalletId", walletId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                wallet = new WalletResponseDto();
                                wallet.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                                wallet.InitialBalance = reader.GetDecimal(reader.GetOrdinal("InitialBalance"));
                                wallet.CurrentBalance = reader.GetDecimal(reader.GetOrdinal("CurrentBalance"));
                                wallet.CurrencyCode = (CurrencyCode)reader.GetInt32(reader.GetOrdinal("CurrencyCode"));
                            }
                        }
                    }
                }

                return wallet;
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task UpdateTransaction(TransactionRequestDto transaction)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.UpdateTransactionQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@TransactionId", transaction.Id);
                        cmd.Parameters.AddWithValue("@StockTicker", transaction.StockTicker);
                        cmd.Parameters.AddWithValue("@Price", transaction.StockPrice);
                        cmd.Parameters.AddWithValue("@Quantity", transaction.Quantity);
                        cmd.Parameters.AddWithValue("@TransactionType", transaction.TransactionType);
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

        public async Task<AccountResponseDto> GetAccountById(Guid accountId)
        {
            AccountResponseDto account = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(SqlQueriesConstants.GetAccountByIdQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@AccountId", accountId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                account = new AccountResponseDto();
                                account.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                                account.Email = reader.GetString(reader.GetOrdinal("Email"));
                                account.WalletId = reader.GetGuid(reader.GetOrdinal("WalletId"));
                                account.Role = (Role)reader.GetInt32(reader.GetOrdinal("Role"));
                                account.DateToDelete = reader.IsDBNull(reader.GetOrdinal("DateToDelete"))
                                    ? null
                                    : reader.GetDateTime(reader.GetOrdinal("DateToDelete"));
                            }
                        }
                    }
                }

                return account;
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }

        public async Task<TransactionRequestDto> GetTransactionById(Guid transactionId)
        {
            TransactionRequestDto transaction = null;
            try
            {
                using(SqlConnection connection = new SqlConnection(ConnectionConstant.connectionString))
                {
                    await connection.OpenAsync();

                    using(SqlCommand cmd = new SqlCommand(SqlQueriesConstants.GetTransactionByIdQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@TransactionId", transactionId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(await reader.ReadAsync())
                            {
                                transaction = new TransactionRequestDto();
                                transaction.Id = reader.GetGuid(reader.GetOrdinal("Id"));
                                transaction.StockTicker = reader.GetString(reader.GetOrdinal("StockTicker"));
                                transaction.StockPrice = reader.GetDecimal(reader.GetOrdinal("Price"));
                                transaction.Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"));
                                transaction.TransactionType = (Domain.Enums.TransactionType)reader.GetInt32(reader.GetOrdinal("TransactionType"));
                                transaction.AccountId = reader.GetGuid(reader.GetOrdinal("AccountId"));
                            }
                        }
                    }
                }
                return transaction;
            }
            catch (SqlException e)
            {
                throw new InvalidOperationException($"Error: {e.Message}. SQL Server error: {e.InnerException?.Message ?? "N/A"}");
            }
        }
    }
}