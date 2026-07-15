using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "AvailableCopies", "Isbn", "PublishedYear", "Title", "TotalCopies" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Robert C. Martin", 3, "978-0132350884", 2008, "Clean Code", 3 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Andrew Hunt", 2, "978-0201616224", 1999, "The Pragmatic Programmer", 2 },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Eric Evans", 0, "978-0321125217", 2003, "Domain-Driven Design", 1 }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Email", "FullName", "IsActive", "PhoneNumber", "RegisteredDate" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), "alice@example.com", "Alice Johnson", true, "0771234567", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "bob@example.com", "Bob Smith", true, null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Borrowings",
                columns: new[] { "Id", "BookId", "BorrowedDate", "DueDate", "MemberId", "ReturnedDate", "Status" },
                values: new object[] { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 15, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444444"), null, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Borrowings",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));
        }
    }
}
