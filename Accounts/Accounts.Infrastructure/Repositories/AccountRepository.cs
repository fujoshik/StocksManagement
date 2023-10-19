using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.DTOs.Account;
using Accounts.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;

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
                Email = dataRow["Email"].ToString()
            };

            return (TOutput)Convert.ChangeType(result, typeof(TOutput));
        }
    }
}
