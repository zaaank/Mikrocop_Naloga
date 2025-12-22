# Generic Template

A Clean Architecture starter template for .NET 9 Web API projects.

## Project Structure

This solution is organized into 5 projects following Clean Architecture principles:

- **Generic.API**: The entry point (Host). Contains Controllers, Middleware, and DI configuration.
- **Generic.Application**: Business logic layer. Contains Services, Validators, and Use Cases.
- **Generic.Domain**: The core domain layer. Contains Entities and Value Objects. No external dependencies.
- **Generic.Infrastructure**: Implementation detail layer. Contains Database Context (EF Core), Repositories, and external service integrations.
- **Generic.Contracts**: Shared DTOs (Data Transfer Objects) for external communication.

## Getting Started

### Prerequisites

- .NET 9 SDK
- SQL Server (LocalDB or Docker)

### Setup

1.  **Clone the repository**.
2.  **Rename the Project** (Optional):
    - Run the rename script to convert "Generic" to your project name (e.g., "UserRepo").
    ```cmd
    .\rename.cmd UserRepo
    ```
3.  **Configure Database**:
    - Open `UserRepo.API/appsettings.json` (or `Generic.API` if you didn't rename).
    - Update the `ConnectionStrings:DefaultConnection`.
    - Default: `Server=.;Database=UserRepoDb;Trusted_Connection=True;Encrypt=False;`
4.  **Run the Application**:
    ```bash
    dotnet run --project UserRepo.API
    ```
4.  **Explore API**:
    - Navigate to `https://localhost:7087/swagger` (port may vary) to see the Swagger UI.

## Development Guide

### Where to add code?

- **New Entity**: Add to `Generic.Domain/Entities`.
- **New Business Logic**: Add a service to `Generic.Application/Services` and interface to `Generic.Application/Abstractions`.
- **New API Endpoint**:
    1.  Create Request/Response DTOs in `Generic.Contracts`.
    2.  Create a Controller in `Generic.API/Controllers`.
    3.  Inject and use your Application Service.
- **New Database Table**:
    1.  Add Entity to `Generic.Infrastructure/Persistence/AppDbContext`.
    2.  Create a migration:
        ```bash
        dotnet ef migrations add <MigrationName> --project Generic.Infrastructure --startup-project Generic.API
        ```
    3.  Update database:
        ```bash
        dotnet ef database update --project Generic.Infrastructure --startup-project Generic.API
        ```

## Architecture Notes

- **Dependency Flow**: API -> Application -> Domain <- Infrastructure
- **Infrastructure Injection**: Infrastructure layer is referenced by API only for DI registration. Application layer defines interfaces that Infrastructure implements.
