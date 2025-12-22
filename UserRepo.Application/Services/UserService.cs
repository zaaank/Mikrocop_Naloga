using System;
using System.Threading.Tasks;
using UserRepo.Application.Common;
using UserRepo.Application.Errors;
using UserRepo.Application.Interfaces;
using UserRepo.Contracts.Requests;
using UserRepo.Contracts.Responses;
using UserRepo.Domain.Entities;
using UserRepo.Domain.Interfaces;

namespace UserRepo.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public UserService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest request)
        {
            if (await _userRepository.GetByUserNameAsync(request.UserName) != null)
            {
                return UserErrors.DuplicateUsername;
            }

            var user = new User
            {
                UserName = request.UserName,
                FullName = request.FullName,
                Email = request.Email,
                MobileNumber = request.MobileNumber,
                Language = request.Language,
                Culture = request.Culture,
                PasswordHash = _passwordService.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);

            return MapToResponse(user);
        }

        public async Task<Result<UserResponse>> GetUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return UserErrors.NotFound;

            return MapToResponse(user);
        }
        
        public async Task<Result<UserResponse>> GetUserByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null) return UserErrors.NotFound;

            return MapToResponse(user);
        }

        public async Task<Result<UserResponse>> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return UserErrors.NotFound;

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.MobileNumber = request.MobileNumber;
            user.Language = request.Language;
            user.Culture = request.Culture;

            await _userRepository.UpdateAsync(user);

            return MapToResponse(user);
        }

        public async Task<Result> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return UserErrors.NotFound;

            await _userRepository.DeleteAsync(id);
            return Result.Success();
        }

        public async Task<Result> ValidatePasswordAsync(string userName, string password)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null) return UserErrors.InvalidCredentials;

            bool isValid = _passwordService.VerifyPassword(password, user.PasswordHash);
            if (!isValid) return UserErrors.InvalidCredentials;

            return Result.Success();
        }

        private static UserResponse MapToResponse(User user)
        {
            return new UserResponse(
                user.Id,
                user.UserName,
                user.FullName,
                user.Email,
                user.MobileNumber,
                user.Language,
                user.Culture
            );
        }
    }
}
