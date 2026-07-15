using System;
using Library.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Api.Infrastructure.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasIndex(m => m.Email).IsUnique();

        builder.HasData(
            new
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                FullName = "Alice Johnson",
                Email = "alice@example.com",
                PhoneNumber = (string?)"0771234567",
                RegisteredDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                FullName = "Bob Smith",
                Email = "bob@example.com",
                PhoneNumber = (string?)null,
                RegisteredDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            }
        );
    }
}
