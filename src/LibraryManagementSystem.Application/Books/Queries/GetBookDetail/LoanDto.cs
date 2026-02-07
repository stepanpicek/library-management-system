namespace LibraryManagementSystem.Application.Books.Queries.GetBookDetail;

public class LoanDto
{
    public Guid Id { get; set; }
    public DateTime BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
}