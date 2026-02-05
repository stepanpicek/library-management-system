using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Books.Commands.BorrowBook;

public class BorrowBookCommandHandler(IApplicationDbContext context) : IRequestHandler<BorrowBookCommand>
{
    public async Task Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        //TODO: Add NotFound exception
        var book = await context.Books.FindAsync(request.BookId, cancellationToken) ??
                   throw new InvalidDataException();
        
        var updated = await context.Books
            .Where(b => b.Id == request.BookId)   
            .ExecuteUpdateAsync(b => b.SetProperty(p => p.AvailableCopies, p => p.AvailableCopies-1),  cancellationToken);

        if (updated == 0)
        {
            //TODO: Add domain exception
            throw new InvalidDataException();
        }

        var loan = new Loan
        {
            Book = book,
            BorrowedAt = DateTime.Now
        };
        
        context.Loans.Add(loan);
        await context.SaveChangesAsync(cancellationToken);
    }
}