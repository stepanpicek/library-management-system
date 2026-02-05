namespace LibraryManagementSystem.Application.Books.Queries.GetBooks;

public class BookDto
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public string Isbn { get; set; }
    public int AvailableCopies { get; set; }
}