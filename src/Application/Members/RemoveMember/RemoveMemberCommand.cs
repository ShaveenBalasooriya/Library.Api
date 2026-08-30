using Application.Abstractions.Messaging;

namespace Application.Members;

public sealed record RemoveMemberCommand(Guid Id) : ICommand;
