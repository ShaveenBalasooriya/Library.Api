using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("members");

        builder.HasKey(m => m.Id);

        builder.ComplexProperty(m => m.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasColumnName("email");
        });

        builder.ComplexProperty(m => m.PhoneNumber, phoneNumberBuilder =>
        {
            phoneNumberBuilder.Property(p => p.Value)
                .HasColumnName("phone_number")
                .HasMaxLength(10);
        });
    }
}
