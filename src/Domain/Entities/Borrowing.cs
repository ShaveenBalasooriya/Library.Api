using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public sealed class Borrowing : Entity
{
    // Gang so time (system time) is something infrastructure related AND is non-deterministic, so we shouldn't be having anything time related here ✋🏽😭  
    public Guid BookId { get; private set; }
    public Guid MemberId { get; private set; }
    public DateTime BorrowedDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnedDate { get; private set; }
    public BorrowingStatus Status { get; private set; }

    private Borrowing(Guid id, Guid bookId, Guid memberId, DateTime borrowedDate, DateTime dueDate) : base(id)
    {
        BookId = bookId;
        MemberId = memberId;
        BorrowedDate = borrowedDate;
        DueDate = dueDate;
        Status = BorrowingStatus.Borrowed;
    }

    public static Result<Borrowing> Create(Guid bookId, Guid memberId)
    {
        if (bookId == Guid.Empty) return Result<Borrowing>.Failure(new Error("Borrowing.BookIdRequired", "The book identifier is required."));

        if (memberId == Guid.Empty) return Result<Borrowing>.Failure(new Error("Borrowing.MemberIdRequired", "The member identifier is required."));

        DateTime borrowedDate = DateTime.UtcNow;
        DateTime dueDate = borrowedDate.AddDays(14);

        return Result<Borrowing>.Success(new Borrowing(Guid.CreateVersion7(), bookId, memberId, borrowedDate, dueDate));

    }

    public Result ReturnBook()
    {
        if (Status == BorrowingStatus.Returned) return Result.Failure(new Error("Borrowing.AlreadyReturned", "The book has already been returned."));

        ReturnedDate = DateTime.Now;
        Status = BorrowingStatus.Returned;

        return Result.Success();
    }

    // This status shall be tracked internally ONLY ✋🏽😔
    public void MarkOverdueIfApplicable()
    {
        // Try switching this to the TimeProvider Abstraction
        if (Status == BorrowingStatus.Borrowed && DateTime.UtcNow > DueDate) Status = BorrowingStatus.Overdue;
    }
}
