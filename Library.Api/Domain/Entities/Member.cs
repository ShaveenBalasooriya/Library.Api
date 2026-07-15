using System;

namespace Library.Api.Domain.Entities;

public class Member
{
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public DateTime RegisteredDate { get; private set; }
    public bool IsActive { get; private set; }

    public Member(string fullName, string email, string? phoneNumber)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        RegisteredDate = DateTime.UtcNow;
        IsActive = true;
    }

    public void Update(string fullName, string? phoneNumber)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
