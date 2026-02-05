namespace LibraryManagementSystem.Application.Loans.Queries.GetLoans;

public class LoanDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
}