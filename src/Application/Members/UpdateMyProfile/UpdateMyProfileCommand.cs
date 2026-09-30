using Application.Abstractions.Messaging;

namespace Application.Members;

public sealed record UpdateMyProfileCommand(string? PhoneNumber) : ICommand;
