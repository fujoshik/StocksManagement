# StocksManagement

## About
This project is an ASP.NET Core-based group of REST APIs (microservices) about a stocks management system where users can register, view latest information about stocks and stock prices, buy and sell them.  
The project supports five roles: **Admin**, **Trial**, **Regular**, **Special** and **VIP** user, based on the balance they have in their digital wallets.  

## Roles
The **Admin** role has access to absolutely everything on the program - he can view, create, delete and update all objects on the site for which such functionality is implemented. He can also create doctor and patient accounts. Each registration on the site creates an account of an ordinary user (patient), and doctor accounts are created only by the administrator.  
**Trial** users are accounts which have not input any sum in their wallets and can use up to 10000 USD to buy stocks for 60 days. After that, they become inactive and can only view their wallet information and account information, unless they deposit enough money to become a Regular user. After 60 more days their accounts are automatically deleted.  
**Regular** users have deposited up to 1000 USD in their walltes.  
**Special** accounts have deposited between 1000 and 5000 USD in their wallets.  
**VIP** accounts have depposited more than 5000 USD in their wallets.  
Users without registration on the platform are only able to view stock prices.  

## Gateway

## Accounts API

### Routes
**Authentication**  
accounts-api/authentication/register -> Register with sum  
accounts-api/authentication/register-trial -> Register without sum (trial)  
accounts-api/authentication/verify -> Endpoint for verifying the registration with the token that was send to the email  
accounts-api/authentication/login -> Login  
accounts-api/authentication/check-token -> Endpoint that checks whether the provided token is valid  

**Wallet**  
accounts-api/wallets/deposit -> Endpoint for depositing money *(CurrencyCode: 0 - USD; 1 - EUR; 2 - BGN)*  
accounts-api/wallets/{id} -> Endpoint for getting wallet information (id is not required; if you do not input id, it will automatically get the logged in user's wallet information)  
accounts-api/wallets/{currency} -> Endpoint for changing the wallet currency, by default the currency is USD *(CurrencyCode: 0 - USD; 1 - EUR; 2 - BGN)*   

## StockAPI

## Settlement API

## Analyzer API