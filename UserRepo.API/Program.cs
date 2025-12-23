using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UserRepo.API.Middleware;
using UserRepo.Application.Interfaces;
using UserRepo.Application.Services;
using UserRepo.Domain.Interfaces;
using UserRepo.Infrastructure.Persistence;
using UserRepo.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION: Logging ---
// We use Serilog for powerful, structured logging to files.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// --- 2. SERVICES: Register Components (Dependency Injection) ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for API Testing and Documentation
builder.Services.AddSwaggerGen(c =>
{
    // Define the security scheme (API Key in Header)
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "ApiKey",
        Type = SecuritySchemeType.ApiKey,
        Description = "API Key needed to access the endpoints. Enter the API Key only."
    });

    // Make Swagger use this security scheme for all requests
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

// Configure the Database Connection
builder.Services.AddDbContext<UserRepoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("UserRepo.Infrastructure")));

// Register our business logic and data access layers
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

var app = builder.Build();

// --- 3. MIDDLEWARE: Configure the Request Pipeline ---
// The order here matters!

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Built-in Serilog request logging (captures basic HTTP info)
app.UseSerilogRequestLogging(); 

app.UseHttpsRedirection();

// Custom Middleware
app.UseMiddleware<ApiKeyAuthMiddleware>(); // Authenticates the request via Header
app.UseMiddleware<LoggingMiddleware>();    // Logs details about the call (who, when, what)

app.UseAuthorization();
app.MapControllers();

// Start the application
app.Run();
