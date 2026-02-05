using MediatR;

namespace LibraryManagementSystem.Application.Loans.Queries.GetLoans;

public record GetLoansQuery(Guid BookId) : IRequest<IReadOnlyCollection<LoanDto>>;