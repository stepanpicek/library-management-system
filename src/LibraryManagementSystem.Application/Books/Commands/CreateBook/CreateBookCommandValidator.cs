using FluentValidation;
using LibraryManagementSystem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Isbn)
            .MustAsync(async (isbn, token) => !await context.Books.AnyAsync(x => x.Isbn == isbn, token))
            .WithMessage("Book with this ISBN already exists");

        RuleFor(x => x.Isbn)
            .NotEmpty()
            .NotNull()
            .Must(IsValidIsbn);

        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull();
        
        RuleFor(x => x.Author)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Year)
            .LessThanOrEqualTo(DateTime.Now.Year);
        
        RuleFor(x => x.AvailableCopies)
            .GreaterThanOrEqualTo(0);
    }

    private bool IsValidIsbn(string isbn)
    {
        var normalized = isbn.Trim().Replace("-", "");
        return (normalized.Length == 10 && IsValidIsbn10(normalized)) ||
               (normalized.Length == 13 && IsValidIsbn13(normalized));
    }
    
    private bool IsValidIsbn10(string isbn)
    {
        var sum = 0;
        for (var i = 0; i < 10; i++)
        {
            if (!char.IsDigit(isbn[i]))
            {
                return false;
            }

            sum += (isbn[i] - '0') * (10 - i);
        }

        return sum % 11 == 0;
    }

    private bool IsValidIsbn13(string isbn)
    {
        var sum = 0;
        for (var i = 0; i < 13; i++)
        {
            if (!char.IsDigit(isbn[i]))
            {
                return false;
            }

            sum += (isbn[i] - '0') * (i % 2 == 0 ? 1 : 3);
        }

        return sum % 10 == 0;
    }
}