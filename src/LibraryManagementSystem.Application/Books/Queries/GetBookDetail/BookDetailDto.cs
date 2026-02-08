namespace LibraryManagementSystem.Application.Books.Queries.GetBookDetail;

public class BookDetailDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public int Year { get; set; }
    public required string Isbn { get; set; }
    public int AvailableCopies { get; set; }
    public IReadOnlyCollection<LoanDto> Loans { get; set; } = [];
}