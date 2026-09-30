using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Members;

internal sealed class CurrentMemberProvider : ICurrentMemberProvider
{
    private readonly IMemberRepository _memberRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CurrentMemberProvider(IMemberRepository memberRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _memberRepository = memberRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<Member>> GetOrCreateCurrentMemberAsync(CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Result<Member>.Failure(new Error("User.Unauthorized", "The request is not authenticated.", ErrorType.Unauthorized));
        }

        if (!_currentUser.IsInRole(Roles.Member))
        {
            return Result<Member>.Failure(new Error("User.NotAMember", "The authenticated user is not a library member.", ErrorType.Forbidden));
        }

        var emailResult = Email.Create(_currentUser.Email);
        if (emailResult.IsFailure)
        {
            return Result<Member>.Failure(emailResult.Error);
        }

        var member = await _memberRepository.GetByIdentityIdAsync(_currentUser.IdentityId, cancellationToken);

        if (member is null)
        {
            var createResult = Member.Create(_currentUser.IdentityId, _currentUser.FullName, emailResult.Value, phoneNumber: null);
            if (createResult.IsFailure)
            {
                return Result<Member>.Failure(createResult.Error);
            }

            member = createResult.Value;
            _memberRepository.Add(member);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Member>.Success(member);
        }

        var syncResult = member.SyncIdentity(_currentUser.FullName, emailResult.Value);
        if (syncResult.IsFailure)
        {
            return Result<Member>.Failure(syncResult.Error);
        }

        if (syncResult.Value)
        {
            _memberRepository.Update(member);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result<Member>.Success(member);
    }
}
