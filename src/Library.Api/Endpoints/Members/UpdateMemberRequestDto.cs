namespace Library.Api.Endpoints.Members;

public sealed record UpdateMemberRequestDto(
    string FullName,
    string? PhoneNumber);
