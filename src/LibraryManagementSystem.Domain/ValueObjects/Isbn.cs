using LibraryManagementSystem.Domain.Common;
using LibraryManagementSystem.Domain.Exceptions;

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
        var normalized = isbn.Replace("-", "");
        if ((normalized.Length == 10 && IsValidIsbn10(normalized)) ||
            (normalized.Length == 13 && IsValidIsbn13(normalized)))
        {
            return new Isbn(isbn);
        }

        throw new InvalidIsbnException(isbn);
    }

    private static bool IsValidIsbn10(string isbn)
    {
        var sum = 0;
        for (var i = 0; i < 10; i++)
        {
            if (!char.IsDigit(isbn[i]))
            {
                return false;
            }

            sum += isbn[i] * (10 - i);
        }

        return sum % 11 == 0;
    }

    private static bool IsValidIsbn13(string isbn)
    {
        var sum = 0;
        for (var i = 0; i < 13; i++)
        {
            if (!char.IsDigit(isbn[i]))
            {
                return false;
            }

            sum += isbn[i] * (i % 2 == 0 ? 1 : 3);
        }

        return sum % 10 == 0;
    }
}