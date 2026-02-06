using LibraryManagementSystem.Application.Books.Commands.BorrowBook;
using LibraryManagementSystem.Application.Books.Commands.ReturnBook;
using LibraryManagementSystem.Application.Books.Queries.GetBooks;
using LibraryManagementSystem.Application.Common.Paging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Web.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(ISender sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<PaginatedList<BookDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBooks([FromQuery] GetBooksQuery query)
    {
        var data = await sender.Send(query);
        return Ok(data);
    }

    [HttpPost("{id:guid}/borrow")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BorrowBook(Guid id)
    {
        await sender.Send(new BorrowBookCommand(id));
        return Created();
    }

    [HttpPost("{id:guid}/return")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReturnBook(Guid id)
    {
        await sender.Send(new ReturnBookCommand(id));
        return Accepted();
    }
}