namespace BuildingBlocks.HelperClasses;

public class Result<T>
{
    protected Result(bool isSuccess, string? message, T? value, Error? error)
    {
        IsSuccess = isSuccess;
        Message = message;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public T? Value { get; init; }
    public Error? Error { get; init; }

    public static Result<T> Success(T value, string message)
    {
        return new Result<T>(true, message, value, null);
    }

    public static Result<T> Failure(Error error)
    {
        return new Result<T>(false, null, default, error);
    }
}