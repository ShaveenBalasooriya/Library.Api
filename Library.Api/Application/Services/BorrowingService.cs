using Library.Api.Application.Exceptions;
using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Borrowings;
using Library.Api.Domain.Entities;

namespace Library.Api.Application.Services;

public class BorrowingService(
    IBorrowingRepository borrowingRepository,
    IBookRepository bookRepository,
    IMemberRepository memberRepository) : IBorrowingService
{
    private const int MaxActiveBorrowings = 3;

    public async Task<IReadOnlyList<BorrowingResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var borrowings = await borrowingRepository.GetAllAsync(ct);
        return [.. borrowings.Select(MapToResponse)];
    }

    public async Task<IReadOnlyList<BorrowingResponse>> GetByMemberIdAsync(Guid memberId, CancellationToken ct = default)
    {
        var member = await memberRepository.GetByIdAsync(memberId, ct)
            ?? throw new NotFoundException($"Member with id '{memberId}' not found.");

        var borrowings = await borrowingRepository.GetByMemberIdAsync(member.Id, ct);
        return [.. borrowings.Select(MapToResponse)];
    }

    public async Task<BorrowingResponse> BorrowAsync(BorrowingBookRequest request, CancellationToken ct = default)
    {
        var book = await bookRepository.GetByIdAsync(request.BookId, ct)
            ?? throw new NotFoundException($"Book with id '{request.BookId}' not found.");

        var member = await memberRepository.GetByIdAsync(request.MemberId, ct)
            ?? throw new NotFoundException($"Member with id '{request.MemberId}' not found.");

        if (!member.IsActive)
            throw new BusinessRuleException("Inactive members cannot borrow books.");

        var activeCount = await borrowingRepository.GetActiveCountByMemberIdAsync(member.Id, ct);
        if (activeCount >= MaxActiveBorrowings)
            throw new BusinessRuleException($"Member cannot borrow more than {MaxActiveBorrowings} books at a time.");

        try
        {
            book.BorrowCopy();
        }
        catch (InvalidOperationException ex)
        {
            throw new BusinessRuleException(ex.Message);
        }

        var borrowing = new Borrowing(book.Id, member.Id);

        await bookRepository.UpdateAsync(book, ct);
        await borrowingRepository.AddAsync(borrowing, ct);

        return MapToResponse(borrowing);
    }

    public async Task<BorrowingResponse> ReturnAsync(Guid borrowingId, CancellationToken ct = default)
    {
        var borrowing = await borrowingRepository.GetByIdAsync(borrowingId, ct)
            ?? throw new NotFoundException($"Borrowing with id '{borrowingId}' not found.");

        var book = await bookRepository.GetByIdAsync(borrowing.BookId, ct)
            ?? throw new NotFoundException($"Book with id '{borrowing.BookId}' not found.");

        try
        {
            borrowing.ReturnBook();
            book.ReturnCopy();
        }
        catch (InvalidOperationException ex)
        {
            throw new BusinessRuleException(ex.Message);
        }

        await borrowingRepository.UpdateAsync(borrowing, ct);
        await bookRepository.UpdateAsync(book, ct);

        return MapToResponse(borrowing);
    }

    private static BorrowingResponse MapToResponse(Borrowing borrowing) => new(
        borrowing.Id, borrowing.BookId, borrowing.MemberId, borrowing.BorrowedDate,
        borrowing.DueDate, borrowing.ReturnedDate, borrowing.Status.ToString());
}
