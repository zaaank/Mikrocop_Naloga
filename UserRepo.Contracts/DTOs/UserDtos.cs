using System;

/* 
 * DTOs (Data Transfer Objects) are used to carry data between the API and the Application layer.
 * They are simple records that define the structure of the data we expect from or send to the user.
 */

namespace UserRepo.Contracts.Requests
{
    /// <summary>
    /// Data required to create a new user.
    /// </summary>
    public record CreateUserRequest(
        string UserName,
        string FullName,
        string Email,
        string? MobileNumber,
        string? Language,
        string? Culture,
        string Password
    );

    /// <summary>
    /// Data allowed to be updated for an existing user.
    /// </summary>
    public record UpdateUserRequest(
        string FullName,
        string Email,
        string? MobileNumber,
        string? Language,
        string? Culture
    );
    
    /// <summary>
    /// Data required to check if a password is correct.
    /// </summary>
    public record ValidatePasswordRequest(
        string UserName,
        string Password
    );
}

namespace UserRepo.Contracts.Responses
{
    /// <summary>
    /// The public information about a user returned by the API.
    /// Notice we NEVER include the password or hash here.
    /// </summary>
    public record UserResponse(
        Guid Id,
        string UserName,
        string FullName,
        string Email,
        string? MobileNumber,
        string? Language,
        string? Culture
    );
}
