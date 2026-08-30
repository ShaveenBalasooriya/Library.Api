using System.Runtime.CompilerServices;
using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Member : Entity
{
    public string FullName { get; private set; }
    public Email Email { get; init; }
    public PhoneNumber? PhoneNumber { get; private set; }
    public DateTime RegisteredDate { get; init; }
    public bool IsActive { get; private set; }

    private Member() : base(Guid.Empty)
    {
        FullName = null!;
        Email = null!;
    }
    private Member(Guid id, string fullName, Email email, PhoneNumber? phoneNumber) : base(id)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        RegisteredDate = DateTime.UtcNow;
        IsActive = true;
    }

    public static Result<Member> Create(string fullName, Email email, PhoneNumber? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(fullName)) return Result<Member>.Failure(new Error("Member.FullNameRequired", "Full name is required.", ErrorType.Validation));

        if (email is null) return Result<Member>.Failure(new Error("Member.EmailRequired", "Email is required.", ErrorType.Validation));

        return Result<Member>.Success(new Member(Guid.CreateVersion7(), fullName, email, phoneNumber));
    }

    public Result Update(string fullName, PhoneNumber? phoneNumber)
    {
        if (fullName is null) return Result.Failure(new Error("Member.FullNameRequired", "Full name is required.", ErrorType.Validation));

        FullName = fullName;
        PhoneNumber = phoneNumber;

        return Result.Success();
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
