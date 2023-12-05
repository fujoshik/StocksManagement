namespace Accounts.Domain.Constants
{
    public class SqlQueryConstants
    {
        public const string USE_STOCKSDB = "USE StocksDB;";

        public const string GET_ACCOUNTS_BY_EMAIL = $@"{USE_STOCKSDB} SELECT * FROM Accounts WHERE Email = @Email";

        public const string UPDATE_ROLE = $@"{USE_STOCKSDB} UPDATE Accounts SET Role = @Role WHERE Id = @Id";

        public const string DELETE_DATE_TO_DELETE = $@"{USE_STOCKSDB} UPDATE Accounts SET DateToDelete = '' WHERE Id = @Id";

        public const string DEPOSIT_SUM_IF_INITIALBALANCE_ABOVE_ZERO = $@"{USE_STOCKSDB} UPDATE Wallets SET CurrentBalance = CurrentBalance + @Sum " +
                    $@"WHERE Id = (SELECT WalletId FROM Accounts WHERE Id = @Id)";

        public const string DEPOSIT_SUM_IF_INITIALBALANCE_IS_ZERO = $@"{USE_STOCKSDB} UPDATE Wallets SET CurrentBalance = @Sum, " +
                        $@"InitialBalance = @Sum " +
                        $@"WHERE Id = (SELECT WalletId FROM Accounts WHERE Id = @Id)";

        public const string GET_CURRENCY_CODE = $"{USE_STOCKSDB} DECLARE @MyTableVar table([test] [int]); " +
                    $@"INSERT INTO @MyTableVar (test) VALUES((SELECT CurrencyCode FROM Wallets WHERE Id = (SELECT WalletId FROM Accounts WHERE Id = @Id))) " +
                    "SELECT * FROM @MyTableVar";

        public const string GET_BALANCE = $"{USE_STOCKSDB} DECLARE @MyTableVar table([test] [decimal]); " +
                    $@"INSERT INTO @MyTableVar (test) VALUES((SELECT @BalanceName FROM Wallets WHERE Id = (SELECT WalletId FROM Accounts WHERE Id = @Id))) " +
                    "SELECT * FROM @MyTableVar";

        public const string CHANGE_CURRENCY_CODE = $"{USE_STOCKSDB} UPDATE Wallets " +
                    $@"SET CurrencyCode = @NewCurrency, CurrentBalance = @NewCurrentBalance, InitialBalance = @NewInitialBalance " +
                    $@"WHERE Id = (SELECT WalletId FROM Accounts WHERE Id = @Id)";

        public const string DELETE_BY_ACCOUNTID = $@"{USE_STOCKSDB} DELETE FROM Users WHERE AccountId = @AccountId";
    }
}
