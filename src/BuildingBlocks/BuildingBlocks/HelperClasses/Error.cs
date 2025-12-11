namespace BuildingBlocks.HelperClasses;

public class Error(string code, string message)
{
    public string Code { get; init; } = code;
    public string Message { get; init; } = message;

    // Predefined errors
    public static Error None => new(string.Empty, string.Empty);
    public static Error NotFound(string message) => new(ErrorCodes.NotFound, message);
    public static Error BadRequest(string message) => new(ErrorCodes.BadRequest, message);
    public static Error InternalServerError(string message) => new(ErrorCodes.InternalServerError, message);
    public static Error ValidationError(string message) => new(ErrorCodes.ValidationError, message);
    public static Error DatabaseError(string message) => new(ErrorCodes.DatabaseError, message);
    public static Error Conflict(string message) => new(ErrorCodes.Conflict, message);
}

public static class ErrorCodes
{
    public const string NotFound = "NotFound";
    public const string BadRequest = "BadRequest";
    public const string InternalServerError = "InternalServerError";
    public const string ValidationError = "ValidationError";
    public const string DatabaseError = "DatabaseError";
    public const string Conflict = "Conflict";
}