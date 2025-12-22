# User Repository Service

A RESTful API service for managing users, secure with API Keys and following Clean Architecture.

## Features

- **User Management**: Create, Read, Update, Delete users.
- **Security**: 
  - API Key Authentication for Clients.
  - BCrypt Password Hashing.
- **Logging**: Detailed per-request logging to daily files.
- **Architecture**: Domain-Driven Design / Clean Architecture.

## Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or Full Instance)

## Setup Instructions

### 1. Database Setup

You have two options to set up the database:

**Option A: EF Core Migrations (Recommended)**
If you have `dotnet-ef` installed:
```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project UserRepo.Infrastructure --startup-project UserRepo.API --output-dir Persistence/Migrations
dotnet ef database update --project UserRepo.Infrastructure --startup-project UserRepo.API
```

**Option B: SQL Script**
Run the provided `setup_db.sql` script against your SQL Server instance to create the database and tables manually.
```bash
sqlcmd -S (localdb)\mssqllocaldb -i setup_db.sql
```

### 2. Configuration

Update `UserRepo.API/appsettings.json` if your SQL Server instance differs from `(localdb)\mssqllocaldb`.

### 3. API Keys

The system uses API Key authentication. A default client is seeded in the SQL script:
- **Client Name**: TestClient
- **API Key**: `secret123`
- **Header**: `X-Api-Key: secret123`

You can add more clients by inserting into the `Clients` table.

## Running the Service

```bash
cd UserRepo.API
dotnet run
```

The service will be available at `https://localhost:7113` (or similar, check console output).
Swagger UI is available at `/swagger`.

## Logging

Logs are stored in `C:\logs`.
Format: `log-yyyyMMdd.txt`.
Logs include: Time, Level, ClientIP, ClientName, Host, Method, Params, Message, ElapsedTime.
Log location can be configured in `appsettings.json` under `Serilog:WriteTo:Args:path`.

## API Usage Examples

**Create User**
```http
POST /api/users
X-Api-Key: secret123
Content-Type: application/json

{
  "userName": "jdoe",
  "fullName": "John Doe",
  "email": "john@example.com",
  "password": "MySecretPassword123!"
}
```

**Validate Password**
```http
POST /api/users/validate-password
X-Api-Key: secret123
Content-Type: application/json

{
  "userName": "jdoe",
  "password": "MySecretPassword123!"
}
```
