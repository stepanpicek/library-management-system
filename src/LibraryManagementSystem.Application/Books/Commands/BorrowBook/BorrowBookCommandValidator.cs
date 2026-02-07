using FluentValidation;
using LibraryManagementSystem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Books.Commands.BorrowBook;

public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.BookId)
            .MustAsync(async (id, token) => await context.Books.AnyAsync(x => x.Id == id, token))
            .WithMessage("Book does not exist");
    }
}