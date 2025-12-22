using System;
using System.Threading.Tasks;
using UserRepo.Domain.Entities;

namespace UserRepo.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<Client?> GetByApiKeyAsync(string apiKey);
    }
}
