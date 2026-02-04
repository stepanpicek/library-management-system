using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Domain.Entities;

public class Loan : Entity
{
    public DateTime BorrowedAt { get; private set; }
    public DateTime? ReturnedAt { get; private set; }
    public Book Book { get; set; }
}