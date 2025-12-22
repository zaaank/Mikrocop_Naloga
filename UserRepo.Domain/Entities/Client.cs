using System;
using System.ComponentModel.DataAnnotations;
using UserRepo.Domain.Common;

namespace UserRepo.Domain.Entities
{
    public class Client : Entity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ApiKey { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
