namespace BuildingBlocks.HelperClasses;

public class Result<T>
{
    public bool IsSuccess { get; init; }
    public T? Value { get; init; }
    public Error? Error { get; init; }
    
    private Result(bool isSuccess, T? value, Error? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public static Result<T> Success(T value) => 
        new(true, value, null);
    
    public static Result<T> Failure(Error error) => 
        new(false, default, error);
}

