using UserRepo.Application.Common;

namespace UserRepo.Application.Errors;

public static class UserErrors
{
    public static readonly Error DuplicateUsername = new("User.DuplicateUsername", "Username already exists.");
    public static readonly Error NotFound = new("User.NotFound", "User not found.");
    public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid username or password.");
}
