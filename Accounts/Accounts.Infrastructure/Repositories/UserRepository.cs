using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.DTOs.User;
using Accounts.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;

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
    }
}
