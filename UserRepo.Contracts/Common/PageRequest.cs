namespace UserRepo.Contracts.Common;

//We can use it later on, if we apply paging to list of users
public record PageRequest(int PageNumber = 1, int PageSize = 10);

