using System;
using System.ComponentModel.DataAnnotations;
using UserRepo.Domain.Common;

namespace UserRepo.Domain.Entities
{
    /// <summary>
    /// Represents a User in the system.
    /// Inherits from Entity to get the standard Id property.
    /// </summary>
    public class User : Entity
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? MobileNumber { get; set; }

        /// <summary>
        /// Preferred language for the user (e.g., "en", "sl").
        /// </summary>
        [MaxLength(10)]
        public string? Language { get; set; }

        /// <summary>
        /// Preferred culture/locale for the user (e.g., "en-US", "sl-SI").
        /// </summary>
        [MaxLength(20)]
        public string? Culture { get; set; }

        /// <summary>
        /// Securely hashed version of the user's password.
        /// WE NEVER STORE PLAIN TEXT PASSWORDS.
        /// </summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
