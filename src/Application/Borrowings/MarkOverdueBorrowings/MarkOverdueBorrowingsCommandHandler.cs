using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.Shared;

namespace Application.Borrowings
{
    internal sealed class MarkOverdueBorrowingsCommandHandler : ICommandHandler<MarkOverdueBorrowingsCommand, int>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkOverdueBorrowingsCommandHandler(IBorrowingRepository borrowingRepository, IUnitOfWork unitOfWork)
        {
            _borrowingRepository = borrowingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(MarkOverdueBorrowingsCommand request, CancellationToken cancellationToken)
        {
            var borrowedRecords = await _borrowingRepository.GetBorrowedAsync(cancellationToken);

            int markedOverdueCount = 0;
            foreach (var borrowing in borrowedRecords)
            {
                borrowing.MarkOverdueIfApplicable();

                if (borrowing.Status == BorrowingStatus.Overdue)
                {
                    _borrowingRepository.Update(borrowing);
                    markedOverdueCount++;
                }
            }

            if (markedOverdueCount > 0)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }

            return Result<int>.Success(markedOverdueCount);
        }
    }
}
