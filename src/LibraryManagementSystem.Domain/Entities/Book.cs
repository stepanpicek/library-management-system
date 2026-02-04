using LibraryManagementSystem.Domain.Common;
using LibraryManagementSystem.Domain.ValueObjects;

namespace LibraryManagementSystem.Domain.Entities;

public class Book : Entity
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Year { get; set; }
    public Isbn Isbn { get; set; }
    public int AvailableCopies { get; set; }
    public ICollection<Loan> Loans { get; set; }
}