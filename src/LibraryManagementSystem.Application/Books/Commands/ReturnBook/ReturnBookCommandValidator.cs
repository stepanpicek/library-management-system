using FluentValidation;
using LibraryManagementSystem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Books.Commands.ReturnBook;

public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.BookId)
            .MustAsync(async (id, token) => await context.Books.AnyAsync(x => x.Id == id, token))
            .WithMessage("Book does not exist");
        
        RuleFor(x => x.BookId)
            .MustAsync(async (id, token) => await context.Loans.AnyAsync(x => x.BookId == id && x.ReturnedAt == null, token))
            .WithMessage("Any opened loan doesn't exist");
    }
}