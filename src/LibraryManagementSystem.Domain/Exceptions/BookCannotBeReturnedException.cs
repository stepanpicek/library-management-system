namespace LibraryManagementSystem.Domain.Exceptions;

public class BookCannotBeReturnedException(Guid bookId) : DomainException($"Book {bookId} cannot be returned");
