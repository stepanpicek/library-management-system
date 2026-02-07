using MediatR;

namespace LibraryManagementSystem.Application.Books.Queries.GetBookDetail;

public record GetBookDetailQuery(Guid Id) : IRequest<BookDetailDto>;