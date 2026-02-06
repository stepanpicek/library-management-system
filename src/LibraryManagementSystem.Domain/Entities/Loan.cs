using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Domain.Entities;

public class Loan : Entity
{
    public DateTime BorrowedAt { get; set; }
    public DateTime? ReturnedAt { get; set; }
    
    public Guid BookId { get; set; }
    public Book Book { get; set; }
}