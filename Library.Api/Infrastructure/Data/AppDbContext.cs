using Library.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Borrowing> Borrowings => Set<Borrowing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasIndex(b => b.Isbn).IsUnique();
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasIndex(m => m.Email).IsUnique();
        });

        modelBuilder.Entity<Borrowing>(entity =>
        {
            entity.HasOne<Book>()
                  .WithMany()
                  .HasForeignKey(br => br.BookId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Member>()
                  .WithMany()
                  .HasForeignKey(br => br.MemberId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
