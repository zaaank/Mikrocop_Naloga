namespace UserRepo.Contracts.Common;

public record ErrorResponse(string Code, string Message, string? Details = null);

