namespace UserRepo.Contracts.Common;

//We can use it later on, if we apply paging to list of users
public record PageResponse<T>(IEnumerable<T> Items, int PageNumber, int PageSize, int TotalCount, int TotalPages);

