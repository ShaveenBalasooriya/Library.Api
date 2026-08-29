using Domain.Enums;
using Domain.Shared;
using static Microsoft.AspNetCore.Http.Results;
using StatusCodes = Microsoft.AspNetCore.Http.StatusCodes;

namespace Library.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert a successful result to Problem Details.");
        }

        return Problem(
            statusCode: result.Error.ErrorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError,
            },
            title: result.Error.Code,
            detail: result.Error.Message,
            extensions: new Dictionary<string, object?>
            {
                {"errors", new[] {result.Error} }
            });
    }
}
