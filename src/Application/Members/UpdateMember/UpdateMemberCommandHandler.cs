using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Members
{
    internal sealed class UpdateMemberCommandHandler : ICommandHandler<UpdateMemberCommand>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(request.Id, cancellationToken);
            if (member is null)
            {
                return Result.Failure(new Error("Member.NotFound", $"Member with ID '{request.Id}' was not found.", ErrorType.NotFound));
            }

            PhoneNumber? phoneNumber = null;
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
                if (phoneNumberResult.IsFailure)
                {
                    return Result.Failure(phoneNumberResult.Error);
                }

                phoneNumber = phoneNumberResult.Value;
            }

            var updateResult = member.Update(request.FullName, phoneNumber);
            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            _memberRepository.Update(member);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
