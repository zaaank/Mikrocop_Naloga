using UserRepo.Application.Common;
using UserRepo.Contracts.Requests;
using UserRepo.Contracts.Responses;

namespace UserRepo.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest request);
        Task<Result<UserResponse>> GetUserAsync(Guid id);
        Task<Result<UserResponse>> GetUserByUserNameAsync(string userName);
        Task<Result<UserResponse>> UpdateUserAsync(Guid id, UpdateUserRequest request);
        Task<Result> DeleteUserAsync(Guid id);
        Task<Result> ValidatePasswordAsync(string userName, string password);
    }
}
