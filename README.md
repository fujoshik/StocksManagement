# Stocks Management

## About
This project is an ASP.NET Core based group of REST APIs (microservices) about a stocks management system where users can register, view latest information about stocks and stock prices, buy and sell them.
The project supports five roles: Admin, Trial, Regular, Special and VIP user, based on the balance they have in their digital wallets.

## Roles
The Admin role has access to absolutely everything on the program - he can view, create, delete and update all objects on the site for which such functionality is implemented. He can also create doctor and patient accounts. Each registration on the site creates an account of an ordinary user (patient), and doctor accounts are created only by the administrator.
Trial users are accounts which have not input any sum in their wallets and can use up to 10000 dollars to buy stocks for 60 days. After that, they become inactive and can only view their wallet information and account information, unless they deposit enough money to become a Regular user. After 60 more days their accounts are automatically deleted.
A Regular account allows 
Special
VIP
Users without registration on the platform are only able to view stock prices.

## Gateway

## Accounts API

## StockAPI

## Settlement API

## Analyzer API