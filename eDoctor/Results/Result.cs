namespace eDoctor.Results;

public class Result
{
    public bool IsSuccess { get; init; }
    public string? Error { get; init; }

    public static Result Success() => new()
    {
        IsSuccess = true
    };

    public static Result Failure(string error) => new()
    {
        IsSuccess = false,
        Error = error
    };
}

public class Result<T> : Result
{
    public T? Value { get; init; }

    public static Result<T> Success(T value) => new()
    {
        IsSuccess = true,
        Value = value
    };

    public new static Result<T> Failure(string error) => new()
    {
        IsSuccess = false,
        Error = error
    };
}

public class Result<TValue, TFallback> : Result<TValue>
{
    public TFallback? Fallback { get; init; }

    public new static Result<TValue, TFallback> Success(TValue value) => new()
    {
        IsSuccess = true,
        Value = value
    };

    public static Result<TValue, TFallback> Failure(string error, TFallback fallback) => new()
    {
        IsSuccess = false,
        Error = error,
        Fallback = fallback
    };
}
