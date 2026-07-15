namespace Library.Api.Contracts.Common;

public record class ErrorResponse(
    int StatusCode,
    string Message,
    string? TraceId = null
);
