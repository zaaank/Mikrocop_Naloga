using System;
using Microsoft.EntityFrameworkCore;
using UserRepo.Domain.Entities;

namespace UserRepo.Infrastructure.Persistence
{
    public class UserRepoDbContext : DbContext
    {
        public UserRepoDbContext(DbContextOptions<UserRepoDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Uniqueness constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.ApiKey)
                .IsUnique();

            // Seed Client
            modelBuilder.Entity<Client>().HasData(
                new Client
                {
                    Id = Guid.Parse("be054320-302a-430c-9602-535352c713b1"),
                    Name = "DefaultClient",
                    ApiKey = "be054320-302a-430c-9602-535352c713b1",
                    IsActive = true,
                    CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc) // Fixed date for deterministic migrations
                }
            );
        }
    }
}
