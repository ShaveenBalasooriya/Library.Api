using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Members
{
    internal sealed class GetAllMembersQueryHandler : IQueryHandler<GetAllMembersQuery, IReadOnlyList<MemberResponse>>
    {
        private readonly IMemberRepository _memberRepository;

        public GetAllMembersQueryHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<Result<IReadOnlyList<MemberResponse>>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
        {
            var members = await _memberRepository.GetAllAsync(cancellationToken);

            var response = members
                .Select(member => new MemberResponse(
                    member.Id,
                    member.FullName,
                    member.Email.Value,
                    member.PhoneNumber?.Value,
                    member.RegisteredDate,
                    member.IsActive))
                .ToList();

            return Result<IReadOnlyList<MemberResponse>>.Success(response);
        }
    }
}
