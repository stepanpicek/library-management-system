using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Books.Commands.ReturnBook;

public class ReturnBookCommandHandler(IApplicationDbContext context) : IRequestHandler<ReturnBookCommand>
{
    public async Task Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var loanId = await context.Loans
                .Where(l => l.BookId == request.BookId && l.ReturnedAt == null)
                .OrderBy(l => l.BorrowedAt)
                .Select(l => l.Id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (loanId == Guid.Empty)
            {
                throw new BookCannotBeReturnedException(request.BookId);
            }
            
            var closed = await context.Loans
                .Where(l => l.Id == loanId && l.ReturnedAt == null)
                .ExecuteUpdateAsync(s =>
                        s.SetProperty(l => l.ReturnedAt, l => DateTime.UtcNow),
                    cancellationToken);

            if (closed == 0)
            {
                throw new BookCannotBeReturnedException(loanId);
            }
        
            await context.Books
                .Where(b => b.Id == request.BookId)
                .ExecuteUpdateAsync(b => 
                        b.SetProperty(p => p.AvailableCopies, p => p.AvailableCopies + 1),
                    cancellationToken);
            
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}