using Application.Abstractions.Messaging;

namespace Application.Members;

public sealed record GetMyProfileQuery : IQuery<MemberProfileResponse>;
