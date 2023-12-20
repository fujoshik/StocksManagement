# StocksManagement

## Overview
This project is an ASP.NET Core-based group of REST APIs (microservices) about a stocks management system where users can register, view latest information about stocks and stock prices, buy and sell them.  
The project supports five roles: **Admin**, **Trial**, **Regular**, **Special** and **VIP** user, based on the balance they have in their digital wallets.  

## Authentication  
StocksManagement uses API keys for authentication. Users need to include their API key in the request headers for authentication purposes.  

### Authentication Header
```http  
Authorization: Bearer YOUR_API_KEY
```

## Roles
- The **Admin** role has access to absolutely everything on the program - he can view, create, delete and update all objects on the site for which such functionality is implemented. He can also create doctor and patient accounts. Each registration on the site creates an account of an ordinary user (patient), and doctor accounts are created only by the administrator.  
- **Trial** users are accounts which have not input any sum in their wallets and can use up to 10000 USD to buy stocks for 60 days. After that, they become inactive and can only view their wallet information and account information, unless they deposit enough money to become a Regular user. After 60 more days their accounts are automatically deleted.  
- **Regular** users have deposited up to 1000 USD in their walltes.  
- **Special** accounts have deposited between 1000 and 5000 USD in their wallets.  
- **VIP** accounts have depposited more than 5000 USD in their wallets.  
- Users without registration on the platform are only able to view stock prices.  

## Error Handling
The API Gateway handles various error scenarios gracefully, providing meaningful error messages and appropriate HTTP status codes to clients.

## Gateway  
### About  
The API Gateway is an ASP.NET Core-based application designed to serve as an entry point for accessing multiple microservices or APIs within an ecosystem. 
It acts as a central point of entry for clients and provides functionalities such as routing, authentication, authorization, load balancing, caching, and more.

### Routes  

- #### Base URL  
The base URL for accessing the StockAPI endpoints is: https://localhost:7071/api/  

- #### Other URLs  
    - #### Authentication  
        - **POST api/authentication/register** -> Register with sum  
        - **POST api/authentication/register-trial** -> Register without sum (trial)  
        - **POST api/authentication/verify?code={authentication_code}** -> Endpoint for verifying the registration with the token that was send to the email  
        - **POST api/authentication/login** -> Login  

    - #### Wallet  
        - **GET api/wallets/{id?}** -> Endpoint for getting wallet information (id is not required; if you do not input id, it will automatically get the logged in user's wallet information)  
        - **POST api/wallets/deposit** -> Endpoint for depositing money *(CurrencyCode: 0 - USD; 1 - EUR; 2 - BGN)*  
        - **POST api/wallets/{currency}** -> Endpoint for changing the wallet currency, by default the currency is USD *(CurrencyCode: 0 - USD; 1 - EUR; 2 - BGN)*   

    - #### User (available only for ADMIN role)  
        - **PUT api/users/{id}** -> Endpoint for updating a user by provided id  
        - **GET api/users/{id}** -> Endpoint for getting a user by provided id
        - **GET api/users** -> Endpoint getting all users in the database  
        - **DELETE api/users/{id}** -> Endpoint for deleting a user by id  

    - #### Stocks  
        - **POST api/stocks/buy-stock?ticker={stock_ticker}&quantity={quantity}** -> Endpoint for buying a certain stock by providing a stock ticker and quantity  
        - **POST api/stocks/sell-stock?ticker={stock_ticker}&quantity={quantity}** -> Endpoint for selling stocks which the user owns  
        - **GET api/stocks/grouped-daily** ->  Gets the daily data from an external API and groups it  
        - **GET api/stocks/get-stock-by-date-and-ticker-from-api?date={date}&stockTicker={stock_ticker}** ->  Gets stocks by date and stock ticker from an external API  
        - **GET api/stocks/get-stock-by-date-and-ticker?date={date}&stockTicker={stock_ticker}** -> Gets stocks by date and stock ticker from a local database  
        - **GET api/stocks/get-stocks-by-date?date={date}** ->  Gets stocks by date from an external API  
        - **GET api/stocks/get-market-characteristics?date={date}** -> gets the market characteristics from a local database  

- #### Analysis
    - **GET api/analysis/average-income?stockTicker={stock_ticker}&date={date}** -> Calculates average income of a user's bought stocks  
    - **GET api/analysis/percentage-change?stockTicker={stock_ticker}&date={date}** -> Calculates the change of a stock's price for the given date  
    - **GET api/analysis/daily-yield-changes?date={date}&stockTicker={stock_ticker}** -> Calculates the price changes of a certain stock for a given period of time  

## Accounts API

### About
Accounts API is an ASP.NET Core-based application responsible for managing user accounts, authentication, authorization, and digital wallets.   
It provides endpoints to handle user registration, login, account management, and operations related to digital wallets.  

### Authentication and Authorization
Accounts API utilizes authentication and authorization mechanisms to secure endpoints and manage user access.  

### Routes
- #### Base URL  
The base URL for accessing the Accounts API endpoints is: https://localhost:7073/accounts-api/  

- #### Other URLs  
    - #### Authentication  
        - **POST accounts-api/authentication/register** -> Register with sum  
        - **POST accounts-api/authentication/register-trial** -> Register without sum (trial)  
        - **POST accounts-api/authentication/verify?code={authentication_code}** -> Endpoint for verifying the registration with the token that was send to the email  
        - **POST accounts-api/authentication/login** -> Login  
        - **POST accounts-api/authentication/check-token?token={bearer_token}** -> Endpoint that checks whether the provided token is valid  

    - #### Wallet  
        - **GET accounts-api/wallets/{id?}** -> Endpoint for getting wallet information (id is not required; if you do not input id, it will automatically get the logged in user's wallet information)  
        - **POST accounts-api/wallets/deposit** -> Endpoint for depositing money *(CurrencyCode: 0 - USD; 1 - EUR; 2 - BGN)*  
        - **POST accounts-api/wallets/{currency}** -> Endpoint for changing the wallet currency, by default the currency is USD *(CurrencyCode: 0 - USD; 1 - EUR; 2 - BGN)*   

    - #### User (available only for ADMIN role)  
        - **PUT accounts-api/users/{id}** -> Endpoint for updating a user by provided id  
        - **GET accounts-api/users/{id}** -> Endpoint for getting a user by provided id
        - **GET accounts-api/users** -> Endpoint getting all users in the database  
        - **DELETE accounts-api/users/{id}** -> Endpoint for deleting a user by id  

    - #### Stocks  
        - **POST accounts-api/stocks/buy-stock?ticker={stock_ticker}&quantity={quantity}** -> Endpoint for buying a certain stock by providing a stock ticker and quantity  
        - **POST accounts-api/stocks/sell-stock?ticker={stock_ticker}&quantity={quantity}** -> Endpoint for selling stocks which the user owns  

    - #### Analysis
        - **GET account-api/analysis/average-income?stockTicker={stock_ticker}** -> Calculates average income of a user's stocks by stock ticker  
        - **GET accounts-api/analysis/percentage-change?stockTicker={stock_ticker}&date={date}** -> Calculates the change of a stock's price for the given date  
        - **GET accounts-api/analysis/daily-yield-changes?date={date}&stockTicker={stock_ticker}** -> Calculates the price changes of a certain stock for a given period of time  
    
### Error Handling
The Accounts API handles various error scenarios gracefully, providing meaningful error messages and appropriate HTTP status codes to clients.  

## Analyzer API  
### About  
Analyzer API is an ASP.NET Core-based application that provides endpoints for analyzing stock prices. 
This API allows users to perform various analytical operations on stock data, such as calculating moving averages, identifying trends, determining volatility, and more.

### Routes

- #### Base URL
The base URL for accessing the AnalyzerAPI endpoints is: https://localhost:7122/api/Calculation/

- #### Other URLs
    - **GET api/Calculation/calculate-current-yield?accountId={account_id}&stockTicker={stock_ticker}&data={date}** -> Calculates average income of a user's bought stocks  
    - **GET api/Calculation/percentage-change?walletId={wallet_id}&stockTicker={stock_ticker}&data={date}** -> Calculates the change of a stock's price for the given date  
    - **GET api/Calculation/daily-yield-changes?date={date}&stockTicker={stock_ticker}&accountId={account_id}** -> Calculates the price changes of a certain stock for a given period of time  

## StockAPI
### About
StockAPI is an ASP.NET Core-based application that provides access to stock market data through a RESTful API.  
This API allows users to retrieve information about stocks, such as stock tickers, historical data, company information, and more. 

### Routes

- #### Base URL
The base URL for accessing the StockAPI endpoints is: https://localhost:7195/api/StockAPI/

- #### Other URLs
    - **GET api/StockAPI/grouped-daily** ->  Gets the daily data from an external API  
    - **GET api/StockAPI/get-stock-by-date-and-ticker-from-api?date={date}&stockTicker={stock_ticker}** ->  Gets stocks by date and stock ticker from an external API  
    - **GET api/StockAPI/get-stock-by-date-and-ticker?date={date}&stockTicker={stock_ticker}** -> Gets stocks by date and stock ticker from a local database; if no records are found in the local database, it gets them from an external API  
    - **GET api/StockAPI/get-stocks-by-date?date={date}** ->  Gets stocks by date from local database
    - **GET api/StockAPI/get-market-characteristics?date={date}** -> Gets the market characteristics from a local database  

## Settlement API
## About
The settlement system is an adaptable financial transaction platform built on ASP.NET Core. 
It efficiently manages financial deals, executes transactions, handles failed transactions, and maintains wallet balances.

## Settlement.API (Assembly)
### Controllers
- **SettlementController:** Handles incoming requests for deal execution by calling upon the `SettlementService`.

### Extensions
- **QuartzExtensions:** Manages scheduled job execution using Quartz.NET, including tasks for daily settlement and failed transaction processing.
- **ServiceConfigurationExstensions:** Configures various services utilized in the system, such as registering dependencies and implementations.

### Application Startup
- The application startup sequence includes service configuration, registration of controllers, and activation of essential components like Swagger documentation and authorization.

## Settlement.Domain

### Abstractions

#### Repository
- **ISettlementRepository:** This repository interface handles database interactions related to settlements, transactions, and wallet management. It offers methods for:
  - Inserting transactions and settlements.
  - Updating wallet balances.
  - Managing failed transactions.
  - Retrieving transaction details and wallet information.

#### Routes
- **IStockRoutes, IWalletRoutes:** These route interfaces provide access to dictionaries containing stock and wallet routes, facilitating communication with external services or APIs.

### Services
- **IConnectionDependencies:** This service interface consolidates essential dependencies required for external communication. It includes:
  - `Http`: HttpClient for HTTP requests.
  - `Wallet` and `Stock`: Route dictionaries for wallet and stock services.
  - `Repository`: Access to the settlement repository.

- **IHttpClientService:** This service interface manages HTTP client interactions for retrieving wallet balances and stock data. The methods include:
  - `GetWalletBalance`: Retrieves the wallet balance for a specified wallet ID.
    - Parameters:
      - `walletId`: The unique identifier of the wallet.
      - `transaction`: Details of the transaction request.
  - `GetStockByDateAndTicker`: Fetches stock information based on a given date and stock ticker symbol.
    - Parameters:
      - `date`: The date for which stock information is requested.
      - `stockTicker`: The symbol representing the specific stock.

- **ISettlementDependencies:** This service interface manages dependencies related to the settlement process. It includes:
  - `Http`: HTTP client service.
  - `Repository`: Access to the settlement repository.
  - `Cache`: Wallet cache service.

- **ISettlementService:** This service interface is responsible for executing deals (transactions) within the settlement system. Method:
  - `ExecuteDeal`: Executes a transaction request and returns the result encapsulated in a `SettlementResponseDto`.

- **IWalletCacheService:** This service interface manages operations related to wallet caching.
- Methods:
  - `GetWalletFromCache`: Retrieves wallet information from the cache based on the specified `walletId`.
  - `SetWalletInCache`: Stores wallet information in the cache for a given `walletId` and `accountId`.

### Calculation
 - **CommissionCalculator:** This class handles the calculation of commission percentages based on different role types within the settlement system.
 - Method:
    - `GetCommisionPercentage`: Retrieves the commission percentage based on the specified `RoleType`. It returns the corresponding percentage value for various roles like Trial, VIP, and Special roles, while using a default percentage for other roles.

### Constants
#### ApiRoutes
 - **ApiRoutesConstants:** This class contains constant strings representing API routes within the settlement system.
 - Fields:
    - `WalletGetRoute`: Represents the route for retrieving wallet information using the endpoint `https://localhost:7073/accounts-api/wallets/{id}`.
    - `StockGetRoute`: Represents the route for fetching stock information using the endpoint `https://localhost:7195/api/StockAPI/get-stock-by-date-and-ticker?date={date}&stockTicker={stockTicker}`. 

#### Messages
- **ResponseMessagesConstants:** This class contains constant strings representing response messages within the settlement system.
- Messages:
  - `InsufficientFunds`: Represents the message for insufficient funds to complete the transaction.
  - `AccountInGoodStanding`: Represents the message indicating that the account is in good standing.
  - `ErrorProcessingDeal`: Represents the message for an error while processing the deal.

#### Queries
- **SqlQueriesConstants:** This class contains constant strings representing SQL queries used within the settlement system.
- Queries:
  - `InsertTransactionQuery`: SQL query for inserting a transaction into the database if it doesn't already exist.
  - `GetTransactionByIdQuery`: SQL query for retrieving a transaction based on its ID.
  - `UpdateWalletBalanceQuery`: SQL query for updating the wallet balance.
  - `GetAllWalletsQuery`: SQL query for retrieving all wallets.
  - `InsertIntoHandledWallets`: SQL query for inserting handled wallets in the database.
  - `GetHandledWalletIdsQuery`: SQL query for retrieving handled wallet IDs.
  - `CreateTableTransactionFailed`: SQL query for creating a table for failed transactions if it doesn't exist.
  - `InsertIntoFailedTransaction`: SQL query for inserting a failed transaction into the database.
  - `GetWalletByIdQuery`: SQL query for retrieving a wallet by its ID.
  - `GetAccountByIdQuery`: SQL query for retrieving an account by its ID.
  - `GetAllTransactions`: SQL query for retrieving all transactions.
  - `GetAllFailedTransactions`: SQL query for retrieving all failed transactions.
  - `DeleteFailedTransaction`: SQL query for deleting a failed transaction.
  - `CheckValidWalletId`: SQL query for checking the validity of a wallet ID.
  - `CreateHandledWalletsTable`: SQL query for creating a table for handled wallets if it doesn't exist.
  - `GetWalletByIdQuery`: SQL query for retrieving a wallet by its ID.
  - `UpdateTransactionQuery`: SQL query for updating a transaction in the database.

- **CommissionPercentageConstant:** Contains the default commission percentage used within the settlement system.
- Constants:
   - `commissionPercentage`: Default commission percentage set to 0.05.

- **ConnectionConstant:** Holds the connection string used to establish a connection with the StocksDB database.
- Constants:
    - `connectionString`: Connection string with server, database name, and authentication details.

- **CronExpressionConstant:** Contains a cron expression used for scheduling jobs within the settlement system.
- Constants:
    - `CronExpression`: Cron expression set to run every 3 minutes.

- **SpecialRolePercentageConstant:** Represents the commission percentage for special role types.
- Constants:
    - `Special`: Commission percentage set to 3 for special roles.

- **TrialRolePercentageConstant:** Represents the commission percentage for trial role types.
- Constants:
    - `Trial`: Commission percentage set to 5 for trial roles.

- **VIPRolePercentageConstant:** Represents the commission percentage for VIP role types.
- Constants:
    - `VIP`: Commission percentage set to 0.5 for VIP roles.

#### Dependencies
- **ConnectionDepenendencies:** This class represents the consolidated dependencies required for external communication within the settlement system.
- Properties:
  - `Http`: Manages the HTTP client for making HTTP requests.
  - `Wallet`: Provides access to wallet-related routes.
  - `Stock`: Provides access to stock-related routes.
  - `Repository`: Access to the settlement repository for managing settlements, transactions, and wallets.

- **SettlementDependencies:** This class represents the dependencies associated with the settlement process within the system.
  - `Http`: Manages HTTP client interactions for retrieving wallet balances and stock data.
  - `Repository`: Provides access to the settlement repository for managing settlements and transactions.
  - `Cache`: Manages operations related to wallet caching.

### DTOs
#### Handled 
- **HandledWalletsDto:** Represents a data transfer object (DTO) for handled wallets within the settlement system.
- Properties:
    - `WalletId`: Unique identifier for the wallet.
    - `AccountId`: Unique identifier for the account associated with the wallet.
    - `TransactionId`: Unique identifier for the transaction linked to the wallet.

#### Settlement
- **SettlementResponseDto:** Represents a data transfer object (DTO) for settlement responses within the settlement system.
- Properties:
    - `Success`: Indicates if the settlement was successful (boolean).
    - `Message`: Describes any additional message regarding the settlement (nullable string).
    - `StockPrice`: Represents the stock price involved in the settlement (decimal).
    - `TotalPrice`: Indicates the total balance affected by the settlement (decimal).

#### Transaction
- **TransactionRequestDto:** Represents a data transfer object (DTO) for transaction requests within the settlement system.
- Properties:
    - `WalletId`: Unique identifier for the wallet associated with the transaction (Guid).
    - `Date`: Date of the transaction (string).
    - `StockTicker`: Symbol representing the stock involved in the transaction (string).
    - `Quantity`: Quantity of stocks involved in the transaction (integer).
    - `AccountId`: Unique identifier for the account associated with the transaction (Guid).
    - `TransactionType`: Type of transaction (enum: TransactionType).
    - `StockPrice`: Price of the stock involved in the transaction (decimal).
    - `Role`: Role type associated with the transaction (enum: RoleType).

- **BaseDto:** Base data transfer object (DTO) used as a foundation for other DTOs within the settlement system.
- Properties:
    - `Id`: Unique identifier for the object (Guid).

### Enums
- **RoleType:** Represents different roles within the settlement system, such as VIP, Special, and Trial roles, used for defining user privileges or commission structures.
- Values:
    - `VIP`: Indicates a VIP role.
    - `Special`: Represents a Special role.
    - `Trial`: Denotes a Trial role.

- **TransactionType:** Represents the nature of a financial transaction within the settlement system.
- Values:
    - `Bought`: Indicates a purchase transaction.
    - `Sold`: Represents a sale transaction.

### Jobs
- **DailySettlementJob:** Executes daily settlement tasks within the settlement system, managing commission deductions from handled wallets.
    - Responsible for deducting trade commissions from handled wallets daily.
    - Method `Execute` handles commission deductions based on associated transactions.

- **ProcessFailedTransactionsJob:** Executes the process for handling failed transactions within the settlement system, attempting to reprocess transactions associated with failed wallet or transaction details.
    - Deals with failed transactions by attempting reprocessing.
    - Method `Execute` resolves failed transactions, updating associated records upon success.

### Routes
- **StockRoutes:** Manages the stock-related routes within the settlement system, facilitating access to stock-specific endpoints.
- Responsibilities:
    - Provides a collection of stock-related routes accessible within the system.
    - Enables easy access to stock-related endpoints for fetching stock information.
- Details:
    - Initializes the stock routes dictionary within the constructor, associating the GET HTTP verb with the stock retrieval endpoint (`ApiRoutesConstants.StockGetRoute`).

- **WalletRoutes:** Manages wallet-specific routes within the settlement system, facilitating access to wallet-related endpoints.
- Responsibilities:
    - Provides a collection of wallet-related routes accessible within the system.
    - Simplifies access to wallet-specific endpoints for retrieving wallet information.
- Details:
    - Initialized within the constructor, associating the GET HTTP verb with the route for retrieving wallet information (`ApiRoutesConstants.WalletGetRoute`).

## Settlement.Domain.Services
- **ConnectionService:** Crucial component facilitating communication with external services. It retrieves wallet balances and stock data for the settlement system. 
- Functionalities:
    - `GetWalletBalance`: Retrieves the balance for a specified wallet using an HTTP GET request to the Accounts API. Upon a successful request, it deserializes the response into a WalletResponseDto object, representing the wallet's balance. 
    In case of a failed request, it logs the transaction as failed.
    - `GetStockByDateAndTicker`: Fetches stock information based on a given date and stock ticker symbol by performing an HTTP GET request to the Stock API. Upon a successful retrieval, it deserializes the response into a Stock object. 
    If the request fails, it raises a HttpRequestException providing details about the failure.
 
- **SettlementService:** Manages the execution of transactions within the settlement system, ensuring accurate processing and updating of transaction-related records.
- Key Functionalities:
    - Validates stock data availability and computes transaction commissions based on stock prices and quantities.
    - Verifies the success of a transaction by checking the user's wallet balance against the required trade commission.
    - Adjusts wallet balances based on transaction types (buy or sell) and updates transaction records upon successful or failed transaction completions.
    - Executes transaction requests by processing stock data, calculating commissions, and managing wallet balances and transaction outcomes.

- **WalletCacheService:** Manages the caching mechanism for wallet-related data within the settlement system, facilitating quick access and retrieval of wallet information.
- Key Functionalities:
    - Retrieves wallet data from the cache if available, fetching it from the repository if not cached.
    - Updates and maintains wallet information in the cache, allowing easy access to recent and frequently accessed wallet data.
    - Utilizes a memory cache to store wallet-related information, enhancing performance by reducing database calls for frequently accessed wallet details.
    - Implements caching strategies by setting expiration times for cached wallet data, ensuring data consistency and refreshing cache entries periodically.

## Settlement.Infrastructure
### Repositories
- **SettlementRepository:** Manages interactions with the database for transaction-related data within the settlement system.
- Key Functionalities:
    - Inserts transaction details into the database, handling various transaction-related information like stock ticker, price, quantity, account ID, etc.
    - Manages failed transactions by storing them in a separate table, including details like wallet ID, stock ticker, price, quantity, account ID, and date.
    - Updates wallet balances in the database, reflecting changes in the current balance based on transaction outcomes.
    - Retrieves failed transactions and handled wallet IDs, allowing for auditing and reprocessing if necessary.
    - Checks the validity of a wallet ID in the database before proceeding with certain operations.
    - Retrieves wallet and account details by their respective IDs for transaction processing and data retrieval purposes.
    - Implements SQL queries to interact with the database using parameterized commands, ensuring data integrity and security.

## Usage
### Steps for inserting failed transaction:
- **1)** ConnectionConstant:
  ```csharp
  namespace Settlement.Domain.Constants
  {
        public class ConnectionConstant
        {
            public const string connectionString = "Enter your actual database connection string...";
        }
   }

- **2)** QuartzExstensions: 
  ```csharp
      using Quartz;
      using Settlement.Domain;
      using Settlement.Domain.Constants;

      namespace Settlement.API.Extensions
      {
            public static class QuartzExstensions
            {
                public static void AddQuartzConfiguration(IServiceCollection services)
                {
                    services.AddQuartz(q =>
                    {
                        // commented out jobs creation
                        /*var dailySettlementJob = JobKey.Create(nameof(DailySettlementJob));
                        q.AddJob<DailySettlementJob>(dailySettlementJob)
                            .AddTrigger(t => t
                                .ForJob(dailySettlementJob)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));*/

                        /*var processFailedTransactions = JobKey.Create(nameof(ProcessFailedTransactionsJob));
                        q.AddJob<ProcessFailedTransactionsJob>(processFailedTransactions)
                            .AddTrigger(t => t
                                .ForJob(processFailedTransactions)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));*/
                    });

                    services.AddQuartzHostedService();
                }
            }
        }

- **3)** Start application and enter this data for `ExecuteDeal` method to test:
      
     ```csharp
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // You don't need to add an ID; it generates itself. You can remove it.
        "walletId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter an invalid wallet ID.
        "date": "string", // Enter a valid date from the Stock API. Check the third GET method when you started the Stock API to take valid data.
        "stockTicker": "string", // Enter a valid stock ticker from the Stock API. Check the third GET method when you started the Stock API to take valid data.
        "quantity": 0, // Integer 
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter valid account ID.
        "transactionType": 0, // Options 0/1 (Bought/Sold)
        "stockPrice": 0, // You can remove it because this information comes from the Stock API with an HTTP GET request. 
        "role": 0 // Options 0/1/2 (VIP, Special, Trial) 
      }
* This method will return false and "Error proccessing deal", but the data from this transaction is now in the `FailedTransactions` table. Stop the application.
- **4)** Go back to step 2:
     ```csharp
        using Quartz;
        using Settlement.Domain;
        using Settlement.Domain.Constants;

        namespace Settlement.API.Extensions
        {
            public static class QuartzExstensions
            {
                public static void AddQuartzConfiguration(IServiceCollection services)
                {
                    services.AddQuartz(q =>
                    {
                        // commented
                        /*var dailySettlementJob = JobKey.Create(nameof(DailySettlementJob));
                        q.AddJob<DailySettlementJob>(dailySettlementJob)
                            .AddTrigger(t => t
                                .ForJob(dailySettlementJob)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));*/

                        // uncomment the second job for ProcessFailedTransactionsJob
                        var processFailedTransactions = JobKey.Create(nameof(ProcessFailedTransactionsJob));
                        q.AddJob<ProcessFailedTransactionsJob>(processFailedTransactions)
                            .AddTrigger(t => t
                                .ForJob(processFailedTransactions)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));
                    });

                    services.AddQuartzHostedService();
                }
            }
        }
* Start the application again and wait 3 minutes to start the job. After the job is done, and this transaction returns true in the `Success` property, your data from `FailedTransactions` is inserted into `Transactions` and `HandledWallets` and deleted from `FailedTransactions`.
* Stop application.

- **5)** Return in step 2 to activate the first job and comment the second job.
    ```csharp
      using Quartz;
      using Settlement.Domain;
      using Settlement.Domain.Constants;

      namespace Settlement.API.Extensions
      {
            public static class QuartzExstensions
            {
                public static void AddQuartzConfiguration(IServiceCollection services)
                {
                    services.AddQuartz(q =>
                    {
                        // uncomment the first job for DailySettlementJob
                        var dailySettlementJob = JobKey.Create(nameof(DailySettlementJob));
                        q.AddJob<DailySettlementJob>(dailySettlementJob)
                            .AddTrigger(t => t
                                .ForJob(dailySettlementJob)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));

                        // comment the second job for ProcessFailedTransactionsJob
                        /*var processFailedTransactions = JobKey.Create(nameof(ProcessFailedTransactionsJob));
                        q.AddJob<ProcessFailedTransactionsJob>(processFailedTransactions)
                            .AddTrigger(t => t
                                .ForJob(processFailedTransactions)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));*/
                    });

                    services.AddQuartzHostedService();
                }
            }
        }
* Start the application again and wait 3 minutes to start the other job.
- **6)** Update Transaction. Comment the two jobs in `QuartzExstensions`.
    ```csharp
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter a valid ID from your database.
        "walletId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter a valid wallet ID.
        "date": "string", // Enter a valid date from the Stock API. Check the third GET method when you started the Stock API to take valid data.
        "stockTicker": "string", // Enter a valid stock ticker from the Stock API. Check the third GET method when you started the Stock API to take valid data.
        "quantity": 0, // Integer  
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter a valid account ID.
        "transactionType": 0, // Options 0/1 (Bought/Sold)
        "stockPrice": 0, // You can remove it because this information comes from the Stock API with an HTTP GET request. 
        "role": 0 // Options 0/1/2 (VIP, Special, Trial) 
      }

### Steps for inserting transaction
- **1)** ConnectionConstant:
  ```csharp
  namespace Settlement.Domain.Constants
  {
        public class ConnectionConstant
        {
            public const string connectionString = "Enter your actual database connection string...";
        }
   }

- **2)** QuartzExstensions: 
  ```csharp
      using Quartz;
      using Settlement.Domain;
      using Settlement.Domain.Constants;

      namespace Settlement.API.Extensions
      {
            public static class QuartzExstensions
            {
                public static void AddQuartzConfiguration(IServiceCollection services)
                {
                    services.AddQuartz(q =>
                    {
                        // commented our jobs
                        /*var dailySettlementJob = JobKey.Create(nameof(DailySettlementJob));
                        q.AddJob<DailySettlementJob>(dailySettlementJob)
                            .AddTrigger(t => t
                                .ForJob(dailySettlementJob)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));*/

                        /*var processFailedTransactions = JobKey.Create(nameof(ProcessFailedTransactionsJob));
                        q.AddJob<ProcessFailedTransactionsJob>(processFailedTransactions)
                            .AddTrigger(t => t
                                .ForJob(processFailedTransactions)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));*/
                    });

                    services.AddQuartzHostedService();
                }
            }
        }

- **3)** Start application and enter this data for `ExecuteDeal` method to test:
      
     ```csharp
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // You don't need to add an ID; it generates itself. You can remove it.
        "walletId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter a valid wallet ID.
        "date": "string", // Enter a valid date from the Stock API. Check the third GET method when you started the Stock API to take valid data.
        "stockTicker": "string", // Enter a valid stock ticker from the Stock API. Check the third GET method when you started the Stock API to take valid data.
        "quantity": 0, // Integer  
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter a valid account ID.
        "transactionType": 0, // Options 0/1 (Bought/Sold)
        "stockPrice": 0, // You can remove it because this information comes from the Stock API with an HTTP GET request. 
        "role": 0 // Options 0/1/2 (VIP, Special, Trial) 
      }
* If success is true will be inserted into `HandledWallets`, if not, it will not be inserted
* Stop application 
- **4)** Go back to step 2 to activate the `DailySettlementJob`
    ```csharp
      using Quartz;
      using Settlement.Domain;
      using Settlement.Domain.Constants;

      namespace Settlement.API.Extensions
      {
            public static class QuartzExstensions
            {
                public static void AddQuartzConfiguration(IServiceCollection services)
                {
                    services.AddQuartz(q =>
                    {
                        // uncomment the first job for DailySettlementJob
                        var dailySettlementJob = JobKey.Create(nameof(DailySettlementJob));
                        q.AddJob<DailySettlementJob>(dailySettlementJob)
                            .AddTrigger(t => t
                                .ForJob(dailySettlementJob)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));

                        // comment the second job for ProcessFailedTransactionsJob
                        /*var processFailedTransactions = JobKey.Create(nameof(ProcessFailedTransactionsJob));
                        q.AddJob<ProcessFailedTransactionsJob>(processFailedTransactions)
                            .AddTrigger(t => t
                                .ForJob(processFailedTransactions)
                                .WithCronSchedule(CronExpressionConstant.CronExpression));*/
                    });

                    services.AddQuartzHostedService();
                }
            }
        }
* Start the application again and wait 3 minutes to start `DailySetttlementJob`. When it's finished comment the job.
- **5)** Update Transaction.
    ```csharp
      {
        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter a valid ID from your database.
        "walletId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter a valid wallet ID 
        "date": "string", // Enter a valid date from the Stock API. Check the third GET method when you started the Stock API to take valid data.
        "stockTicker": "string", // Enter a valid stock ticker from the Stock API. Check the third GET method when you started the Stock API to take valid data.
        "quantity": 0, // Integer 
        "accountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6", // Enter a valid account ID.
        "transactionType": 0, // Options 0/1 (Bought/Sold)
        "stockPrice": 0, // You can remove it because this information comes from the Stock API with an HTTP GET request. 
        "role": 0 // Options 0/1/2 (VIP, Special, Trial) 
      }
## Routes
- **POST settlements-api/** -> Executes the transaction of buying or selling stocks