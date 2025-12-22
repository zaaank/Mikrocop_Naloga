namespace UserRepo.Contracts.Common;

public record PageResponse<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount, int TotalPages);

