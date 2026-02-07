using LibraryManagementSystem.Application.Common.Paging;
using MediatR;

namespace LibraryManagementSystem.Application.Books.Queries.GetBooks;

public record GetBooksQuery(string? Text = null, int? Page = null, int? Size = null) : IRequest<PaginatedList<BookDto>>;