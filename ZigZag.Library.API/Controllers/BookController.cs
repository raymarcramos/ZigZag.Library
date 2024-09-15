using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using ZigZag.Library.API.Services.Services;
using ZigZag.Library.DataAccess.Models;
using ZigZag.Library.DataAccess.Models.Requests;

namespace ZigZag.Library.API.Controllers;

[ApiController]
public class BookController(IBookService bookService) : Controller
{
   
    [HttpGet]
    [Route("books")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Book>))]
    [SwaggerOperation(Summary = "Get all books", Description = "Retrieve a list of all books in the library.")]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await bookService.GetAllAsync();

        return Ok(books);
    }

    [HttpPost]
    [Route("book")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Book))]
    [SwaggerOperation(Summary = "Add a new book", Description = "Add a new book to the library.")]
    public async Task<IActionResult> CreateBook([FromBody, Required] BookRequest bookRequest)
    {
        var createdBook = await bookService.CreateAsync(bookRequest);

        return CreatedAtAction(nameof(CreateBook), new { id = createdBook.Id }, createdBook);
    }

    [HttpGet]
    [Route("book/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get a specific book", Description = "Retrieve a specific book by its ID.")]
    public async Task<IActionResult> GetBookById([FromRoute, Required] int id)
    {
        var book = await bookService.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPut]
    [Route("book/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Book))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Update a book", Description = "Update an existing book by its ID.")]
    public async Task<IActionResult> UpdateBook([FromRoute, Required] int id, [FromBody, Required] BookRequest bookRequest)
    {
        var updatedBook = await bookService.UpdateAsync(id, bookRequest);

        if (updatedBook == null)
        {
            return NotFound();
        }

        return Ok(updatedBook);
    }
}
