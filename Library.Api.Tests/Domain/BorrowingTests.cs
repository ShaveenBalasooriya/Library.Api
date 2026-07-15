using Library.Api.Domain.Entities;

namespace Library.Api.Tests.Domain;

public class BorrowingTests
{
    [Fact]
    public void ReturnBook_Throws_WhenAlreadyReturned()
    {
        // Given
        var borrowing = new Borrowing(Guid.NewGuid(), Guid.NewGuid());
        // When
        borrowing.ReturnBook();
        // Then
        Assert.Throws<InvalidOperationException>(() => borrowing.ReturnBook());
    }
}
