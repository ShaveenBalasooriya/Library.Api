using System;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Api.Infrastructure.Configurations;

public class BorrowingConfiguration : IEntityTypeConfiguration<Borrowing>
{
    public void Configure(EntityTypeBuilder<Borrowing> builder)
    {
        builder.HasOne<Book>()
            .WithMany()
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Member>()
            .WithMany()
            .HasForeignKey(br => br.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(
            new
            {
                Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                BookId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                MemberId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                BorrowedDate = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                DueDate = new DateTime(2026, 7, 15, 0, 0, 0, DateTimeKind.Utc),
                ReturnedDate = (DateTime?)null,
                Status = BorrowingStatus.Borrowed
            }
        );
    }
}
