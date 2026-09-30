using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Members
{
    internal sealed class UpdateMyProfileCommandHandler : ICommandHandler<UpdateMyProfileCommand>
    {
        private readonly ICurrentMemberProvider _currentMemberProvider;
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMyProfileCommandHandler(
            ICurrentMemberProvider currentMemberProvider,
            IMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _currentMemberProvider = currentMemberProvider;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateMyProfileCommand request, CancellationToken cancellationToken)
        {
            var memberResult = await _currentMemberProvider.GetOrCreateCurrentMemberAsync(cancellationToken);
            if (memberResult.IsFailure)
            {
                return Result.Failure(memberResult.Error);
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

            var member = memberResult.Value;
            var updateResult = member.UpdateProfile(phoneNumber);
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
