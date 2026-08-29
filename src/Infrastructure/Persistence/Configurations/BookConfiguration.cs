using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("books");

        builder.HasKey(b => b.Id);

        builder.ComplexProperty(b => b.Isbn, isbnBuilder =>
        {
            isbnBuilder.Property(i => i.Value)
                .HasColumnName("isbn")
                .HasMaxLength(13);
        });

        builder.HasIndex(b => b.Isbn.Value).IsUnique();

        builder.ComplexProperty(b => b.PublishedYear, publishedYearBuilder =>
        {
            publishedYearBuilder.Property(p => p.Value)
                .HasColumnName("published_year");
        });

        builder.ComplexProperty(b => b.Copies, copiesBuilder =>
        {
            copiesBuilder.Property(c => c.Total).HasColumnName("total_copies");
            copiesBuilder.Property(c => c.Available).HasColumnName("available_copies");
        });
    }
}
