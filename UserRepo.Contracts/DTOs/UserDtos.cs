using System;

namespace UserRepo.Contracts.Requests
{
    public record CreateUserRequest(
        string UserName,
        string FullName,
        string Email,
        string? MobileNumber,
        string? Language,
        string? Culture,
        string Password
    );

    public record UpdateUserRequest(
        string FullName,
        string Email,
        string? MobileNumber,
        string? Language,
        string? Culture
    );
    
    public record ValidatePasswordRequest(
        string UserName,
        string Password
    );
}

namespace UserRepo.Contracts.Responses
{
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
