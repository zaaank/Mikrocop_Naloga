using System;
using System.ComponentModel.DataAnnotations;
using UserRepo.Domain.Common;

namespace UserRepo.Domain.Entities
{
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

        [MaxLength(10)]
        public string? Language { get; set; } // e.g., "en", "sl"

        [MaxLength(20)]
        public string? Culture { get; set; } // e.g., "en-US", "sl-SI"

        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
