namespace Catalog.API.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result);
        }

        // Create error response with details if available
        // var errorResponse = result.Error!.Details != null && result.Error.Details.Any()
        //     ? new
        //     {
        //         isSuccess = result.IsSuccess,
        //         error = result.Error.Code,
        //         message = result.Error.Message,
        //         details = result.Error.Details
        //     }
        //     : new
        //     {
        //         isSuccess = result.IsSuccess,
        //         error = result.Error.Code,
        //         message = result.Error.Message
        //     } as object;

        return result.Error!.Code switch
        {
            ErrorCodes.NotFound => Results.NotFound(result),
            ErrorCodes.Conflict => Results.Conflict(result),
            ErrorCodes.BadRequest or ErrorCodes.ValidationError => Results.BadRequest(result),

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