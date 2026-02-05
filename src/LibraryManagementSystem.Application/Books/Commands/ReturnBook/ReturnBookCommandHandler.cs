using MediatR;

namespace LibraryManagementSystem.Application.Books.Commands.ReturnBook;

public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand>
{
    public Task Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}