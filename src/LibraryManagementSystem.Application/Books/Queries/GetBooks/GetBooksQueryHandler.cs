using LibraryManagementSystem.Application.Common.Extensions;
using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Application.Common.Paging;
using LibraryManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Books.Queries.GetBooks;

public class GetBooksQueryHandler(IApplicationDbContext context) : IRequestHandler<GetBooksQuery, PaginatedList<BookDto>>
{
    public Task<PaginatedList<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Book> books = context.Books
            .AsNoTracking()
            .OrderBy(b => b.Title);

        if (request.Text is not null)
        {
            var text = request.Text.Trim();
            books = books.Where(b => EF.Functions.Like(b.Title, $"%{text}%") 
                                     || EF.Functions.Like(b.Author, $"%{text}%") 
                                     || EF.Functions.Like(b.Isbn, $"%{text}%") );
        }

        return books
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Year = b.Year,
                Isbn = b.Isbn,
                AvailableCopies = b.AvailableCopies
            })
            .Paginate(request.Page, request.Size);
    }
}