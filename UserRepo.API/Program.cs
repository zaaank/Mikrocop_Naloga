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

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "ApiKey",
        Type = SecuritySchemeType.ApiKey,
        Description = "API Key needed to access the endpoints. Enter the API Key only."
    });

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

// DbContext
builder.Services.AddDbContext<UserRepoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("UserRepo.Infrastructure"))); // Ensure migrations are in Infrastructure

// Application Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging(); // Optional, but good for default request logging. Our custom middleware does specific fields.

app.UseHttpsRedirection();

// Custom
app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseMiddleware<LoggingMiddleware>();


app.UseAuthorization();

app.MapControllers();

app.Run();
