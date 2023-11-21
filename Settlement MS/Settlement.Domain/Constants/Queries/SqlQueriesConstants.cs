using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.Constants.Queries
{
    public class SqlQueriesConstants
    {
        public const string InsertTransactionQuery = @"
        IF NOT EXISTS (SELECT * FROM TRANSACTIONS)
        BEGIN
            INSERT INTO Transactions (StockTicker, Price, Quantity, TransactionType, AccountId)
            VALUES (@StockTicker, @Price, @Quantity, @TransactionType, @AccountId)
        END";

        public const string UpdateWalletBalanceQuery = @"
        UPDATE Wallets SET CurrentBalance = @NewBalance WHERE Id = @WalletId";

        public const string GetAllWalletsQuery = @"SELECT * FROM Wallets";

        public const string CheckExistingWalletRecord = @"
        IF NOT EXISTS (SELECT WalletId FROM HandledWallets WHERE WalletId = @WalletId)
        BEGIN
            INSERT INTO HandledWallets (WalletId) VALUES (@WalletId)
        END";

        public const string GetHandledWalletIdsQuery = @"SELECT WalletId FROM HandledWallets";
    }
}
