namespace BuildingBlocks.HelperClasses;

public class Error(
    string code,
    string message,
    IDictionary<string, object?>? details = null)
{
    public string Code { get; init; } = code;
    public string Message { get; init; } = message;
    public IDictionary<string, object?>? Details { get; init; } = details;

    // Predefined errors
    public static Error None => new(string.Empty, string.Empty);

    public static Error NotFound(string message,
        IDictionary<string, object?>? details = null)
    {
        return new Error(ErrorCodes.NotFound, message, details);
    }

    public static Error BadRequest(string message,
        IDictionary<string, object?>? details = null)
    {
        return new Error(ErrorCodes.BadRequest, message, details);
    }


    public static Error Conflict(string message,
        IDictionary<string, object?>? details = null)
    {
        return new Error(ErrorCodes.Conflict, message, details);
    }

    // Enhanced validation error with details
    public static Error ValidationError(string message,
        IDictionary<string, object?>? details = null)
    {
        return new Error(ErrorCodes.ValidationError, message, details);
    }

    public static Error DatabaseError(string message,
        IDictionary<string, object?>? details = null)
    {
        return new Error(ErrorCodes.DatabaseError, message, details);
    }

    public static Error InternalServerError(string message,
        IDictionary<string, object?>? details)
    {
        return new Error(ErrorCodes.InternalServerError, message, details);
    }
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