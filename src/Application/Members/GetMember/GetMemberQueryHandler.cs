using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Shared;

namespace Application.Members
{
    internal sealed class GetMemberQueryHandler : IQueryHandler<GetMemberQuery, MemberResponse>
    {
        private readonly IMemberRepository _memberRepository;

        public GetMemberQueryHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Result<MemberResponse>> Handle(GetMemberQuery request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(request.Id, cancellationToken);
            if (member is null)
            {
                return Result<MemberResponse>.Failure(new Error("Member.NotFound", $"Member with ID '{request.Id}' was not found.", ErrorType.NotFound));
            }

            var response = new MemberResponse(
                member.Id,
                member.FullName,
                member.Email.Value,
                member.PhoneNumber?.Value,
                member.RegisteredDate,
                member.IsActive);

            return Result<MemberResponse>.Success(response);
        }
    }
}
