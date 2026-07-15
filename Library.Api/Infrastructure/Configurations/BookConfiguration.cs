using System;
using Library.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Api.Infrastructure.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasIndex(b => b.Isbn).IsUnique();

        builder.HasData(
            new
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Title = "Clean Code",
                Author = "Robert C. Martin",
                Isbn = "978-0132350884",
                PublishedYear = 2008,
                TotalCopies = 3,
                AvailableCopies = 3
            },
            new
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "The Pragmatic Programmer",
                Author = "Andrew Hunt",
                Isbn = "978-0201616224",
                PublishedYear = 1999,
                TotalCopies = 2,
                AvailableCopies = 2
            },
            new
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "Domain-Driven Design",
                Author = "Eric Evans",
                Isbn = "978-0321125217",
                PublishedYear = 2003,
                TotalCopies = 1,
                AvailableCopies = 0
            }
        );
    }
}
