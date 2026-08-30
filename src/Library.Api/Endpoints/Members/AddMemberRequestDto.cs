namespace Library.Api.Endpoints.Members;

public sealed record AddMemberRequestDto(
    string FullName,
    string Email,
    string? PhoneNumber);
