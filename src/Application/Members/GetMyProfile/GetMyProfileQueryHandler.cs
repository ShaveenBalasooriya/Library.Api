using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Members
{
    internal sealed class GetMyProfileQueryHandler : IQueryHandler<GetMyProfileQuery, MemberProfileResponse>
    {
        private readonly ICurrentMemberProvider _currentMemberProvider;

        public GetMyProfileQueryHandler(ICurrentMemberProvider currentMemberProvider)
        {
            _currentMemberProvider = currentMemberProvider;
        }

        public async Task<Result<MemberProfileResponse>> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        {
            var memberResult = await _currentMemberProvider.GetOrCreateCurrentMemberAsync(cancellationToken);
            if (memberResult.IsFailure)
            {
                return Result<MemberProfileResponse>.Failure(memberResult.Error);
            }

            var member = memberResult.Value;

            var response = new MemberProfileResponse(
                member.Id,
                member.FullName,
                member.Email.Value,
                member.PhoneNumber?.Value,
                member.RegisteredDate,
                member.IsActive,
                member.IsProfileComplete);

            return Result<MemberProfileResponse>.Success(response);
        }
    }
}
