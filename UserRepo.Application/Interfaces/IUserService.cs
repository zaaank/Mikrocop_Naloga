using System;
using System.Threading.Tasks;
using UserRepo.Contracts.Requests;
using UserRepo.Contracts.Responses;

namespace UserRepo.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse?> GetUserAsync(Guid id);
        Task<UserResponse?> GetUserByUserNameAsync(string userName);
        Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> ValidatePasswordAsync(string userName, string password);
    }
}
