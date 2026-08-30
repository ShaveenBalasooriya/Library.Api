using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Members
{
    internal sealed class AddMemberCommandHandler : ICommandHandler<AddMemberCommand, Guid>
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddMemberCommandHandler(IMemberRepository memberRepository, IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddMemberCommand request, CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);
            if (emailResult.IsFailure)
            {
                return Result<Guid>.Failure(emailResult.Error);
            }

            PhoneNumber? phoneNumber = null;
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
            {
                var phoneNumberResult = PhoneNumber.Create(request.PhoneNumber);
                if (phoneNumberResult.IsFailure)
                {
                    return Result<Guid>.Failure(phoneNumberResult.Error);
                }

                phoneNumber = phoneNumberResult.Value;
            }

            bool isUnique = await _memberRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken);
            if (!isUnique)
            {
                return Result<Guid>.Failure(new Error("Member.DuplicateEmail", $"A member with email '{request.Email}' already exists.", ErrorType.Conflict));
            }

            var memberResult = Member.Create(request.FullName, emailResult.Value, phoneNumber);
            if (memberResult.IsFailure)
            {
                return Result<Guid>.Failure(memberResult.Error);
            }

            var member = memberResult.Value;

            _memberRepository.Add(member);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(member.Id);
        }
    }
}
