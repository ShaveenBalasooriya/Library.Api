namespace Library.Api.Contracts.Common;

public record ValidationErrorResponse(
    int StatusCode,
    string Message,
    List<ValidationErrorItem> Errors
);

public record ValidationErrorItem(
    string Field,
    string Message
);