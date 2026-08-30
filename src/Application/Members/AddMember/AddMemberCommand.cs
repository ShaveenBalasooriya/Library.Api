using Application.Abstractions.Messaging;

namespace Application.Members;

public sealed record AddMemberCommand(
    string FullName,
    string Email,
    string? PhoneNumber) : ICommand<Guid>;
