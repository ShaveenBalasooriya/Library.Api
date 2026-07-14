using System.ComponentModel.DataAnnotations;
using Library.Api.Contracts.Common;

namespace Library.Api.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();

        if (argument is null)
            return await next(context);

        var validationContext = new ValidationContext(argument);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            argument, validationContext, validationResults, validateAllProperties: true);

        if (!isValid)
        {
            var errors = validationResults
                .SelectMany(
                    r => r.MemberNames.DefaultIfEmpty(string.Empty),
                    (r, member) => new ValidationErrorItem(member, r.ErrorMessage ?? "Invalid value."))
                .ToList();

            return Results.BadRequest(new ValidationErrorResponse(400, "Validation failed", errors));
        }

        return await next(context);
    }
}
