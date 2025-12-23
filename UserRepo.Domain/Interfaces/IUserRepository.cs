using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UserRepo.Domain.Entities;

namespace UserRepo.Domain.Interfaces
{
    /// <summary>
    /// Interface for User-related database operations.
    /// Defines the contract that the Infrastructure layer must implement.
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUserNameAsync(string userName);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
