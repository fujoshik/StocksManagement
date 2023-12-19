namespace Settlement.Domain.Constants.Queries
{
    public class SqlQueriesConstants
    {
        public const string UseStocksDB = @"USE STOCKSDB";
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

        public const string CreateTableTransactionFailed = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FailedTransactions')
        BEGIN
            CREATE TABLE FailedTransactions (
            WalletId uniqueidentifier PRIMARY KEY,
            StockTicker varchar(255),
			Price decimal(16,4),
			Quantity int,
			DateOfTransaction datetime NOT NULL DEFAULT GETDATE(),
			TransactionType int,
			AccountId uniqueidentifier,
            Date varchar(255),
            );
        END";

        public const string InsertIntoFailedTransaction = @"
        IF NOT EXISTS (SELECT * FROM FailedTransactions)
        BEGIN
            INSERT INTO FailedTransactions (WalletId, StockTicker, Price, Quantity, TransactionType, AccountId, Date)
            VALUES (@WalletId, @StockTicker, @Price, @Quantity, @TransactionType, @AccountId, @Date)
        END";


        public const string GetWalletByIdQuery = @"
        SELECT * FROM Wallets
        WHERE Id = @WalletId";

        public const string GetAccountById = @"
        SELECT * FROM Accounts
        WHERE Id = @AccountId";

        public const string GetAllTransactions = @"
        SELECT * FROM Transactions";

        public const string GetAllFailedTransactions = @"
        SELECT * FROM FailedTransactions";

        public const string DeleteFailedTransaction = @"
        DELETE FROM FailedTransactions WHERE WalletId = @WalletId";

        public const string CheckValidWalletId = @"
        SELECT * FROM Accounts WHERE WalletId = @WalletId";

        public const string CreateHandledWalletsTable = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'HandledWallets')
        BEGIN
            CREATE TABLE HandledWallets (
            Id uniqueidentifier PRIMARY KEY DEFAULT NEWID(),
            WalletId uniqueidentifier,
            AccountId uniqueidentifier,
            TransactionId uniqueidentifier,
            CONSTRAINT FK_HandledWallets_Wallets FOREIGN KEY (WalletId) REFERENCES Wallets(Id),
            CONSTRAINT FK_HandledWallets_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id),
            CONSTRAINT FK_HandledWallets_Transactions FOREIGN KEY (TransactionId) REFERENCES Transactions(Id)
            );
        END";

        public const string GetWalltByIdQuery = @"
        SELECT Id, InitialBalance, CurrentBalance, CurrencyCode
        FROM Wallets
        WHERE Id = @WalletId";

        public const string UpdateTransactionQuery = @"
        UPDATE Transactions
        SET StockTicker = @StockTicker,
            Price = @Price,
            Quantity = @Quantity,
            TransactionType = @TransactionType,
            AccountId = @AccountId
        WHERE Id = @TransactionId";


    }
}
