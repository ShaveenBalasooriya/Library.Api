namespace Library.Api.Contracts.Members;

public record class MemberResponse(
    Guid Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    DateTime RegisteredDate,
    bool IsActive
);