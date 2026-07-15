using Library.Api.Domain.Entities;

namespace Library.Api.Tests.Domain;

public class BookTests
{
    [Fact]
    public void BorrowCopy_Throws_WhenNoAvailableCopiesRemain()
    {
        // Given
        var book = new Book("Clean Code", "Robert Martin", "978-0132350884", 2008, totalCopies: 1);
        // When
        book.BorrowCopy();
        // Then
        Assert.Throws<InvalidOperationException>(() => book.BorrowCopy());
    }

    [Fact]
    public void ReturnCopy_IncrementsAvailableCopies()
    {
        // Given
        var book = new Book("Clean Code", "Robert Martin", "978-0132350884", 2008, totalCopies: 5);
        // When
        book.BorrowCopy();
        book.ReturnCopy();
        // Then
        Assert.Equal(5, book.AvailableCopies);
    }

}
