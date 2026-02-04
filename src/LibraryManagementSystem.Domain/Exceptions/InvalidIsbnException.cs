namespace LibraryManagementSystem.Domain.Exceptions;

public class InvalidIsbnException(string isbn) : Exception($"ISBN {isbn} is invalid.");