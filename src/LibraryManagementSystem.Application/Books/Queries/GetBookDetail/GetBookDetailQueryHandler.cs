using LibraryManagementSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Books.Queries.GetBookDetail;

public class GetBookDetailQueryHandler(IApplicationDbContext context) : IRequestHandler<GetBookDetailQuery, BookDetailDto>
{
    public Task<BookDetailDto> Handle(GetBookDetailQuery request, CancellationToken cancellationToken)
    {
        return context.Books
            .AsNoTracking()
            .Include(b => b.Loans)
            .Select(b => new BookDetailDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Year = b.Year,
                Isbn = b.Isbn,
                AvailableCopies = b.AvailableCopies,
                Loans = b.Loans.Select(l => new LoanDto
                {
                    Id = l.Id,
                    BorrowedAt = l.BorrowedAt,
                    ReturnedAt = l.ReturnedAt,
                }).ToList(),
            })
            .FirstAsync(b => b.Id == request.Id, cancellationToken);
    }
}