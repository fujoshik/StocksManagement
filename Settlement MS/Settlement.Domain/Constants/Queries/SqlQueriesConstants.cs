namespace Settlement.Domain.Constants.Queries
{
    public class SqlQueriesConstants
    {
        public const string InsertTransactionQuery = @"
        IF NOT EXISTS (SELECT Id FROM Transactions WHERE Id = @Id)
        BEGIN
            INSERT INTO Transactions (Id, StockTicker, Price, Quantity, TransactionType, AccountId)
            VALUES (@Id, @StockTicker, @Price, @Quantity, @TransactionType, @AccountId)
        END";

        public const string GetTransactionByIdQuery = @"
        SELECT * FROM Transactions
        WHERE Id = @TransactionId";

        public const string UpdateWalletBalanceQuery = @"
        UPDATE Wallets SET CurrentBalance = @NewBalance WHERE Id = @WalletId";

        public const string GetAllWalletsQuery = @"SELECT * FROM Wallets";

        public const string InsertIntoHandledWallets = @"
        IF NOT EXISTS (SELECT TransactionId FROM HandledWallets WHERE TransactionId = @TransactionId)
        BEGIN
            INSERT INTO HandledWallets (WalletId, AccountId, TransactionId) 
            VALUES (@WalletId, @AccountId, @TransactionId)
        END";

        public const string GetHandledWalletIdsQuery = @"SELECT * FROM HandledWallets";

        //            TransactionId uniqueidentifier NULL,
        //            CONSTRAINT FK_FailedTransactions_Transactions FOREIGN KEY (TransactionId) REFERENCES Transactions(Id)
        public const string CreateTableTransactionFailed = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FailedTransactions')
        BEGIN
            CREATE TABLE FailedTransactions (
            Id uniqueidentifier PRIMARY KEY DEFAULT NEWID(),
            WalletId uniqueidentifier,
            StockTicker varchar(255),
			Price decimal(16,4),
			Quantity int,
			DateOfTransaction datetime NOT NULL DEFAULT GETDATE(),
			TransactionType int,
			AccountId uniqueidentifier,
            Date varchar(255),
            CONSTRAINT FK_FailedTransactions_Accounts FOREIGN KEY (AccountId) REFERENCES Accounts(Id)
            );
        END";

        public const string InsertIntoFailedTransaction = @"
        INSERT INTO FailedTransactions (WalletId, StockTicker, Price, Quantity, TransactionType, AccountId, Date)
        VALUES (@WalletId, @StockTicker, @Price, @Quantity, @TransactionType, @AccountId, @Date)";

        public const string GetWalletByIdQuery = @"
        SELECT * FROM Wallets
        WHERE Id = @WalletId";

        public const string GetAccountByIdQuery = @"
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
