namespace UserRepo.Application.Common;

/// <summary>
/// A generic pattern to return success or failure from services without using exceptions for control flow.
/// It contains a status (IsSuccess) and an Error if failed.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        // Enforce logical consistency: Success cannot have an error, Failure MUST have an error.
        if (isSuccess && error != Error.None)
            throw new InvalidOperationException();

        if (!isSuccess && error == Error.None)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    
    // Allows implicit conversion from an Error to a failed Result.
    public static implicit operator Result(Error error) => Failure(error);
}

/// <summary>
/// A version of Result that also carries a return value on success.
/// </summary>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Returns the value if success, otherwise throws an exception (should be checked via IsSuccess first).
    /// </summary>
    public TValue Value => IsSuccess 
        ? _value! 
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    public static implicit operator Result<TValue>(TValue value) => Success(value);
    public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
}

