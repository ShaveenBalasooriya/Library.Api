using Application.Abstractions.Messaging;

namespace Application.Members;

public sealed record GetAllMembersQuery : IQuery<IReadOnlyList<MemberResponse>>;
