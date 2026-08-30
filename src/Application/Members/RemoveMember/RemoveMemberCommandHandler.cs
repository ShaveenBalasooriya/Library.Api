using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Shared;

namespace Application.Members
{
    internal sealed class RemoveMemberCommandHandler : ICommandHandler<RemoveMemberCommand>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(request.Id, cancellationToken);
            if (member is null)
            {
                return Result.Failure(new Error("Member.NotFound", $"Member with ID '{request.Id}' was not found.", ErrorType.NotFound));
            }

            _memberRepository.Remove(member);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
