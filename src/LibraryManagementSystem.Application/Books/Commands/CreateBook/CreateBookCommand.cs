using MediatR;

namespace LibraryManagementSystem.Application.Books.Commands.CreateBook;

public record CreateBookCommand(string Title, string Author, int Year, string Isbn, int AvailableCopies) : IRequest;