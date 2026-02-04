using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Domain.ValueObjects;

public class Isbn : ValueObject
{
    private Isbn(string isbn)
    {
        Value = isbn;
    }
    
    public string Value { get; init; }

    public static Isbn Parse(string isbn)
    {
        return new Isbn(isbn);
    }
}