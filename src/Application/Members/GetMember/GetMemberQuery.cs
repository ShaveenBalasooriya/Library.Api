using Application.Abstractions.Messaging;

namespace Application.Members;

public sealed record GetMemberQuery(Guid Id) : IQuery<MemberResponse>;
