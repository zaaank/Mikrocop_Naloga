namespace UserRepo.Application.Common;

/// <summary>
/// A simple record to represent an error with a Code and a Message.
/// This avoids using strings for errors and makes handling more structured.
/// </summary>
public sealed record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}

