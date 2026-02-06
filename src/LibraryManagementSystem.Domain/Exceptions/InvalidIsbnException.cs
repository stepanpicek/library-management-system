namespace LibraryManagementSystem.Domain.Exceptions;

public class InvalidIsbnException(string isbn) : DomainException($"ISBN {isbn} is invalid.");