namespace LibraryManagementSystem.Application.Books.Queries.GetBookDetail;

public class BookDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string Isbn { get; set; }
    public int AvailableCopies { get; set; }
    public IReadOnlyCollection<LoanDto> Loans { get; set; }
}