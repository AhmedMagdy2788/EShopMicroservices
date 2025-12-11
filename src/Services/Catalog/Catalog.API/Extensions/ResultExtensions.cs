namespace Catalog.API.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        // Create a consistent error response object
        var errorResponse = new
        {
            error = result.Error!.Code,
            message = result.Error.Message
        };

        return result.Error!.Code switch
        {
            ErrorCodes.NotFound => Results.NotFound(errorResponse),
            ErrorCodes.Conflict => Results.Conflict(errorResponse),
            ErrorCodes.BadRequest or ErrorCodes.ValidationError => Results.BadRequest(errorResponse),

            ErrorCodes.DatabaseError or ErrorCodes.InternalServerError => Results.Problem(
                title: result.Error.Code,
                detail: result.Error.Message,
                statusCode: StatusCodes.Status500InternalServerError
            ),

            _ => Results.Problem(
                title: "An unexpected error occurred",
                detail: result.Error.Message,
                statusCode: StatusCodes.Status500InternalServerError
            )
        };
    }
}