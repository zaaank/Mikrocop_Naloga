using System;
using Microsoft.EntityFrameworkCore;
using UserRepo.Domain.Entities;

namespace UserRepo.Infrastructure.Persistence
{
    /// <summary>
    /// Gateway to the SQL database using Entity Framework Core.
    /// Maps our Domain Entities to Database Tables.
    /// </summary>
    public class UserRepoDbContext : DbContext
    {
        public UserRepoDbContext(DbContextOptions<UserRepoDbContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuration for User table
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Configuration for Client table
            modelBuilder.Entity<Client>()
                .HasIndex(c => c.ApiKey)
                .IsUnique();

            // Seed Client (Development/Testing Data)
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = Guid.Parse("be054320-302a-430c-9602-535352c713b1"),
                    Name = "DefaultClient",
                    ApiKey = "be054320-302a-430c-9602-535352c713b1",
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
