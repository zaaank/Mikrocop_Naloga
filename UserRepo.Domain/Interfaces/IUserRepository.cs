using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UserRepo.Domain.Entities;

namespace UserRepo.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUserNameAsync(string userName);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
