namespace Application.Members;

public sealed record MemberProfileResponse(
    Guid Id,
    string FullName,
    string Email,
    string? PhoneNumber,
    DateTime RegisteredDate,
    bool IsActive,
    bool IsProfileComplete);
