using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Constants;
using Accounts.Domain.DTOs.User;
using Accounts.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Accounts.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration configuration)
            : base(configuration)
        {
            this.TableName = nameof(User) + "s";
        }

        protected override TOutput DataRowToEntity<TOutput>(DataRow dataRow)
        {
            var result = new UserResponseDto()
            {
                Id = Guid.Parse(dataRow["Id"].ToString()),
                FirstName = dataRow["FirstName"].ToString(),
                LastName = dataRow["LastName"].ToString(),
                Age = int.Parse(dataRow["Age"].ToString()),
                PhoneNumber = dataRow["PhoneNumber"].ToString(),
                Country = dataRow["Country"].ToString(),
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
                SqlCommand cmd = new SqlCommand(SqlQueryConstants.DELETE_BY_ACCOUNTID, connection);
                cmd.Parameters.Add(new SqlParameter("@AccountId", accountId));
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
