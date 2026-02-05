using MediatR;

namespace LibraryManagementSystem.Application.Books.Commands.BorrowBook;

public record BorrowBookCommand(Guid BookId) : IRequest;