using MediatR;

namespace LibraryManagementSystem.Application.Loans.Queries.GetLoans;

public class GetLoansQueryHandler : IRequestHandler<GetLoansQuery, IReadOnlyCollection<LoanDto>>
{
    public Task<IReadOnlyCollection<LoanDto>> Handle(GetLoansQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}