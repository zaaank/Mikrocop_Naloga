using System;
using System.Threading.Tasks;
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

        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            // Basic validation (could be moved to FluentValidation)
            if (await _userRepository.GetByUserNameAsync(request.UserName) != null)
            {
                throw new InvalidOperationException("Username already exists.");
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

        public async Task<UserResponse?> GetUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? null : MapToResponse(user);
        }
        
        public async Task<UserResponse?> GetUserByUserNameAsync(string userName)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            return user == null ? null : MapToResponse(user);
        }

        public async Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.MobileNumber = request.MobileNumber;
            user.Language = request.Language;
            user.Culture = request.Culture;

            await _userRepository.UpdateAsync(user);

            return MapToResponse(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            await _userRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> ValidatePasswordAsync(string userName, string password)
        {
            var user = await _userRepository.GetByUserNameAsync(userName);
            if (user == null) return false;

            return _passwordService.VerifyPassword(password, user.PasswordHash);
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
