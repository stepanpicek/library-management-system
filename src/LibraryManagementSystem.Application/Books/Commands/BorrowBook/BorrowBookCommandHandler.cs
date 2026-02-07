using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Books.Commands.BorrowBook;

public class BorrowBookCommandHandler(IApplicationDbContext context) : IRequestHandler<BorrowBookCommand>
{
    public async Task Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var updated = await context.Books
                .Where(b => b.Id == request.BookId && b.AvailableCopies > 0)
                .ExecuteUpdateAsync(b => 
                        b.SetProperty(p => p.AvailableCopies, p => p.AvailableCopies - 1),
                    cancellationToken);

            if (updated == 0)
            {
                throw new BookCannotBeBorrowedException(request.BookId);
            }

            var loan = new Loan
            {
                BookId = request.BookId,
                BorrowedAt = DateTime.UtcNow
            };

            context.Loans.Add(loan);
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}