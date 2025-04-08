# aspnetcore-efcore-db-webapi

A scalable NET Solutions Architecture (ASP.NET Core + EF Core) code-first database example, where C# classes define the database schema. EF Core tracks schema changes via migrations for version control. This is designed with enterprise patterns (SOLID, Clean Architecture). Demonstrates:  
- SQLServer/EF Core migrations  
- RESTful API best practices  
- CI/CD-ready structure

1. Project Setup
Create a new ASP.NET Core Web API project:
```
dotnet new webapi -n EFCoreDemo
cd EFCoreDemo
```
2. Install EF Core Packages
```
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer  # For SQL Server
```
3. File Structure of defining model in Product.cs, Creating DbContext in AppDbContext.cs & Configuring Dbcontext in Program.cs
```
aspnetcore-efcore-db-webapi/
├── Models/
│   └── Product.cs
├── Data/
│   └── AppDbContext.cs
├── appsettings.json
├── Program.cs
└── aspnetcore-efcore-db-webapi.csproj
```
4. Run the Project

a. Apply migrations

Migrations translate C# model classes (Product.cs) into actual database tables (e.g., a Products table in SQL Server). Also, track changes to schema (versioning). 
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```
This creates an app.db SQLite file and the Migrations/ folder.

b. Start the API:
```
dotnet run
```
c. Test the endpoint:

GET https://localhost:5001/api/products
Returns products.

POST https://localhost:5001/api/products
Adds a new product (send JSON body).

```
[
  { "id": 1, "name": "P1", "price": 49.99 },
  { "id": 2, "name": "P2", "price": 199.99 }
]
```
