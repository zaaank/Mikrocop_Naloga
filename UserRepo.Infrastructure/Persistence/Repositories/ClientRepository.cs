using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserRepo.Domain.Entities;
using UserRepo.Domain.Interfaces;

namespace UserRepo.Infrastructure.Persistence.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly UserRepoDbContext _context;

        public ClientRepository(UserRepoDbContext context)
        {
            _context = context;
        }

        public async Task<Client?> GetByApiKeyAsync(string apiKey)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.ApiKey == apiKey && c.IsActive);
        }
    }
}
