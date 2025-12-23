using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserRepo.Domain.Entities;
using UserRepo.Domain.Interfaces;

namespace UserRepo.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Implementation of IClientRepository using Entity Framework Core.
    /// Handles retrieval of API Client credentials.
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private readonly UserRepoDbContext _context;

        public ClientRepository(UserRepoDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetByApiKeyAsync(string apiKey)
        {
            // Only return the client if they are marked as Active.
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.ApiKey == apiKey && c.IsActive);
        }
    }
}
