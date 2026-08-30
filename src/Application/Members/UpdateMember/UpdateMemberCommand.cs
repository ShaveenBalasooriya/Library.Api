using Application.Abstractions.Messaging;

namespace Application.Members;

public sealed record UpdateMemberCommand(
    Guid Id,
    string FullName,
    string? PhoneNumber) : ICommand;
