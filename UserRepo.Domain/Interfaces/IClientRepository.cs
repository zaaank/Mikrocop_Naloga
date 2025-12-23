using System;
using System.Threading.Tasks;
using UserRepo.Domain.Entities;

namespace UserRepo.Domain.Interfaces
{
    /// <summary>
    /// Interface for API Client-related database operations.
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>
        /// Retrieves a client by their unique API Key.
        /// </summary>
        Task<Client?> GetByApiKeyAsync(string apiKey);
    }
}
