using LibraryManagementSystem.Application.Books.Commands.CreateBook;
using MediatR;

namespace LibraryManagementSystem.Application.Books.Commands.ReturnBook;

public record ReturnBookCommand(Guid BookId) : IRequest<CreateBookCommand>, IRequest;