namespace Application.Members;

public sealed record MemberResponse(
    Guid Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    DateTime RegisteredDate,
    bool IsActive);
