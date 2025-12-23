using System;
using System.ComponentModel.DataAnnotations;
using UserRepo.Domain.Common;

namespace UserRepo.Domain.Entities
{
    /// <summary>
    /// Represents an API Client that is allowed to access our services.
    /// Each Client has a unique ApiKey used for authentication.
    /// </summary>
    public class Client : Entity
    {
        /// <summary>
        /// Friendly name of the client (e.g., "MobileApp", "WebFrontend").
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The unique key the client must send in the request header to authenticate.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// If false, the client cannot access the API even with a valid key.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// When the client record was first created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
