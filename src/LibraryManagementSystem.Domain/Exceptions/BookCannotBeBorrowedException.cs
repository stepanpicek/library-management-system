namespace LibraryManagementSystem.Domain.Exceptions;

public class BookCannotBeBorrowedException(Guid bookId) : DomainException($"Book {bookId} cannot be borrowed");