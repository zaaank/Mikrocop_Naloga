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

**EF Core Migrations (Recommended)**
If you have `dotnet-ef` installed:
```bash
dotnet tool install --global dotnet-ef
dotnet ef database update --project UserRepo.Infrastructure --startup-project UserRepo.API
```

### 2. Configuration

Update `UserRepo.API/appsettings.json` if your SQL Server instance differs from `localhost\\sql2019`.

### 3. API Keys

The system uses API Key authentication. A default client is seeded via migrations ( For testing purposes only ):
- **Client Name**: DefaultClient
- **API Key**: `be054320-302a-430c-9602-535352c713b1`
- **Header Name**: `ApiKey`

### 4. Using Swagger with API Key

1.  Run the application.
2.  Navigate to `https://localhost:7113/swagger/index.html` (check console for actual port).
3.  Click the **Authorize** button (top right).
4.  In the value field, enter the API Key: `be054320-302a-430c-9602-535352c713b1`.
5.  Click **Authorize** and then **Close**.
6.  You can now call the protected endpoints.

## Running the Service ( In VS just set .API as startup project and press F5 )

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
ApiKey: be054320-302a-430c-9602-535352c713b1
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
ApiKey-Key: be054320-302a-430c-9602-535352c713b1
Content-Type: application/json

{
  "userName": "jdoe",
  "password": "MySecretPassword123!"
}
```
