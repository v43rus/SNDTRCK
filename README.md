# SNDTRCK

SNDTRCK is a demo e-commerce site for vinyl lovers. It runs on **ASP.NET Core 8** with Razor views and uses **Entity Framework Core** for data access. 
User accounts and roles are provided by **ASP.NET Identity**, giving admins a dashboard to manage products, orders and users.

## Features

- Browse albums by genre, search with live suggestions and see tracklists from the Discogs API.
- Add items to a cookie-based shopping cart that updates dynamically.
- Secure checkout that saves orders to a SQL Server database.
- Admin area for CRUD operations on products, orders and user accounts.

## Setup

1. Clone the repository.
2. Set the connection string named `DefaultConnection` in `appsettings.json` or via user secrets.
3. Restore dependencies and create the database:
   ```bash
   dotnet restore
   dotnet ef database update
