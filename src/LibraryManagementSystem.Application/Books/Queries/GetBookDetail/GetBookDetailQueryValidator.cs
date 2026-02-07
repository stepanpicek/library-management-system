using FluentValidation;
using LibraryManagementSystem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Application.Books.Queries.GetBookDetail;

public class GetBookDetailQueryValidator : AbstractValidator<GetBookDetailQuery>
{
    public GetBookDetailQueryValidator(IApplicationDbContext context)
    {
        RuleFor(x => x.Id)
            .MustAsync(async (id, token) => await context.Books.AnyAsync(x => x.Id == id, token))
            .WithMessage("Book does not exist");
            
    }
}