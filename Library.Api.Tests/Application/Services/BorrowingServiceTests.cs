using Library.Api.Application.Exceptions;
using Library.Api.Application.Interfaces;
using Library.Api.Application.Services;
using Library.Api.Contracts.Borrowings;
using Library.Api.Domain.Entities;
using NSubstitute;

namespace Library.Api.Tests.Application.Services;

public class BorrowingServiceTests
{
    [Fact]
    public async Task BorrowAsync_Throws_WhenMemberIsInactive()
    {
        // Given
        var bookRepo = Substitute.For<IBookRepository>();
        var memberRepo = Substitute.For<IMemberRepository>();
        var borrowingRepo = Substitute.For<IBorrowingRepository>();

        var book = new Book("Clean Code", "Robert Martin", "978-0132350884", 2008, totalCopies: 5);
        var member = new Member("Shaveen Balasooriya", "test@email.com", "090-930-290");
        // When
        member.Deactivate();
        bookRepo.GetByIdAsync(book.Id).Returns(book);
        memberRepo.GetByIdAsync(member.Id).Returns(member);
        // Then
        var service = new BorrowingService(borrowingRepo, bookRepo, memberRepo);
        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.BorrowAsync(new BorrowingBookRequest(book.Id, member.Id)));
    }

    [Fact]
    public async Task BorrowAsync_Throws_WhenMemberHasThreeActiveBorrowings()
    {
        // Given
        var bookRepo = Substitute.For<IBookRepository>();
        var memberRepo = Substitute.For<IMemberRepository>();
        var borrowRepo = Substitute.For<IBorrowingRepository>();

        var book = new Book("Clean Code", "Robert Martin", "978-0132350884", 2008, totalCopies: 5);
        var member = new Member("Shaveen Balasooriya", "test@email.com", "090-930-290");
        // When
        bookRepo.GetByIdAsync(book.Id).Returns(book);
        memberRepo.GetByIdAsync(member.Id).Returns(member);
        borrowRepo.GetActiveCountByMemberIdAsync(member.Id).Returns(3);
        // Then
        var service = new BorrowingService(borrowRepo, bookRepo, memberRepo);

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.BorrowAsync(new BorrowingBookRequest(book.Id, member.Id)));
    }
}
