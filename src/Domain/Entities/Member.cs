using System.Runtime.CompilerServices;
using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Member : Entity
{
    public string IdentityId { get; private init; }
    public string FullName { get; private set; }
    public Email Email { get; private set; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public DateTime RegisteredDate { get; init; }
    public bool IsActive { get; private set; }
    public bool IsProfileComplete => PhoneNumber is not null;

    private Member() : base(Guid.Empty)
    {
        IdentityId = null!;
        FullName = null!;
        Email = null!;
    }
    private Member(Guid id, string identityId, string fullName, Email email, PhoneNumber? phoneNumber) : base(id)
    {
        IdentityId = identityId;
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        RegisteredDate = DateTime.UtcNow;
        IsActive = true;
    }

    public static Result<Member> Create(string identityId, string fullName, Email email, PhoneNumber? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(identityId)) return Result<Member>.Failure(new Error("Member.IdentityIdRequired", "Identity id is required.", ErrorType.Validation));

        if (string.IsNullOrWhiteSpace(fullName)) return Result<Member>.Failure(new Error("Member.FullNameRequired", "Full name is required.", ErrorType.Validation));

        if (email is null) return Result<Member>.Failure(new Error("Member.EmailRequired", "Email is required.", ErrorType.Validation));

        return Result<Member>.Success(new Member(Guid.CreateVersion7(), identityId, fullName, email, phoneNumber));
    }

    public Result<bool> SyncIdentity(string fullName, Email email)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return Result<bool>.Failure(new Error("Member.FullNameRequired", "Full name is required.", ErrorType.Validation));

        if (email is null) return Result<bool>.Failure(new Error("Member.EmailRequired", "Email is required.", ErrorType.Validation));

        bool changed = false;

        if (!string.Equals(FullName, fullName, StringComparison.Ordinal))
        {
            FullName = fullName;
            changed = true;
        }

        if (Email != email)
        {
            Email = email;
            changed = true;
        }

        return Result<bool>.Success(changed);
    }

    public Result UpdateProfile(PhoneNumber? phoneNumber)
    {
        PhoneNumber = phoneNumber;

        return Result.Success();
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
