using LibraryManagementSystem.Application.Common.Paging;
using MediatR;

namespace LibraryManagementSystem.Application.Books.Queries.GetBooks;

public record GetBooksQuery(string? Text, int? Page, int? Size) : IRequest<PaginatedList<BookDto>>;