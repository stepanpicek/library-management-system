using LibraryManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Data.Seeds;

public static class BookSeeds
{
    public static Book Book1 => new()
    {
        Title = "Malý princ",
        Author = "Antoine de Saint-Exupéry",
        Year = 1943,
        Isbn = "978-80-00-07359-0",
        AvailableCopies = 9,
        Loans = new List<Loan>
        {
            new()
            {
                BorrowedAt = DateTime.Now.AddMonths(-1),
                ReturnedAt = DateTime.Now.AddDays(-1),
            },
            new ()
            {
                BorrowedAt = DateTime.Now.AddDays(-10),
            }
        }
    };

    public static Book Book2 => new()
    {
        Title = "1984",
        Author = "George Orwell",
        Year = 1949,
        Isbn = "978-80-7335-647-7",
        AvailableCopies = 0,
        Loans = new List<Loan>
        {
            new ()
            {
                BorrowedAt = DateTime.Now.AddDays(-10),
            },
            new ()
            {
                BorrowedAt = DateTime.Now.AddDays(-1),
            }
        }
    };
    
    public static Book Book3 => new()
    {
        Title = "R.U.R.",
        Author = "Karel Čapek",
        Year = 1920,
        Isbn = "80-86216-46-2",
        AvailableCopies = 5,
        Loans = new List<Loan>
        {
            new()
            {
                BorrowedAt = DateTime.Now.AddMonths(-1),
                ReturnedAt = DateTime.Now.AddDays(-1),
            }
        }
    };

    public static Task SeedBooksAsync(DbContext context)
    {
        return context.Set<Book>().AddRangeAsync(Book1, Book2, Book3);
    }

    public static void SeedBooks(DbContext context)
    {
        context.Set<Book>().AddRange(Book1, Book2, Book3);
    }
}