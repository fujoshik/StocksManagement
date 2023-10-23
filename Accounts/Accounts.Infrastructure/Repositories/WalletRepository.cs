using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.DTOs.User;
using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;
using Accounts.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using System.Data;

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
                CurrentBalance = decimal.Parse(dataRow["CurrentBalance"].ToString()),
                Currency = (CurrencyCode)int.Parse(dataRow["CurrencyCode"].ToString())
            };

            return (TOutput)Convert.ChangeType(result, typeof(TOutput));
        }
    }
}
