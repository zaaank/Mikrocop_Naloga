using UserRepo.Application.Interfaces;
using BCrypt.Net;

namespace UserRepo.Application.Services
{
    /// <summary>
    /// Wrapper service for password security.
    /// Uses BCrypt for industry-standard hashing.
    /// </summary>
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            // BCrypt handles salts automatically.
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
