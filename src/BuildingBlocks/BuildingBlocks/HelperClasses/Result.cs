namespace BuildingBlocks.HelperClasses;

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public T? Value { get; init; }
    public Error? Error { get; init; }
    
    private Result(bool isSuccess, string? message,  T? value, Error? error)
    {
        IsSuccess = isSuccess;
        Message = message;
        Value = value;
        Error = error;
    }
    
    public static Result<T> Success(T value, string message) => 
        new(true, message, value, null);
    
    public static Result<T> Failure(Error error) => 
        new(false, null, default, error);
}

