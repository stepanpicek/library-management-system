using LibraryManagementSystem.Application.Common.Interfaces;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.ValueObjects;
using MediatR;

namespace LibraryManagementSystem.Application.Books.Commands.CreateBook;

public class CreateBookCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateBookCommand>
{
    public Task Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Year = request.Year,
            Isbn = Isbn.Parse(request.Isbn),
            AvailableCopies = request.AvailableCopies
        };
        
        context.Books.Add(book);
        return context.SaveChangesAsync(cancellationToken);
    }
}