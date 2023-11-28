using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.Enums;
using Accounts.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Accounts.Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        public AccountRepository(IConfiguration configuration)
            : base(configuration)
        {
            this.TableName = nameof(Account) + "s";
        }

        protected override TOutput DataRowToEntity<TOutput>(DataRow dataRow)
        {
            var result = new AccountResponseDto()
            {
                Id = Guid.Parse(dataRow["Id"].ToString()),
                Email = dataRow["Email"].ToString(),
                WalletId = Guid.Parse(dataRow["WalletId"].ToString()),
                Role = (Role)int.Parse(dataRow["Role"].ToString()),
                DateToDelete = dataRow["DateToDelete"].ToString() == null ? null : DateTime.Parse(dataRow["DateToDelete"].ToString()) 
            };

            return (TOutput)Convert.ChangeType(result, typeof(TOutput));
        }

        public async Task<List<AccountDto>> GetAccountsByEmail(string email)
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($@"USE StocksDB; SELECT * FROM {TableName} WHERE Email = @Email", connection);
                cmd.Parameters.Add(new SqlParameter("@Email", email));
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    dataTable.Load(reader);
                }
            }

            return dataTable
                .AsEnumerable()
                .Select(x => new AccountDto
                {
                    Id = Guid.Parse(x["Id"].ToString()),
                    Email = x["Email"].ToString(),
                    PasswordHash = x["PasswordHash"].ToString(),
                    PasswordSalt = x["PasswordSalt"].ToString(),
                    Role = (Role)int.Parse(x["Role"].ToString()),
                    DateToDelete = x["DateToDelete"].ToString() == null ? null : DateTime.Parse(x["DateToDelete"].ToString())
                })
                .ToList();
        }

        public async Task UpdateRoleAsync(Guid id, int role)
        {
            await CreateDbIfNotExist();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($@"USE StocksDB; UPDATE {TableName} SET Role = @Role WHERE Id = @Id", connection);
                cmd.Parameters.Add(new SqlParameter("@Role", role));
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteDateToDeleteAsync(Guid id)
        {
            await CreateDbIfNotExist();

            using (var connection = new SqlConnection(_dbConnectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand($@"USE StocksDB; UPDATE {TableName} SET DateToDelete = '' WHERE Id = @Id", connection);
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
