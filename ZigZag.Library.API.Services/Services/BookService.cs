using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZigZag.Library.API.Services.Exceptions;
using ZigZag.Library.DataAccess;
using ZigZag.Library.DataAccess.Models;
using ZigZag.Library.DataAccess.Models.Requests;

namespace ZigZag.Library.API.Services.Services;
public interface IBookService
{
    public Task<IEnumerable<Book>> GetAllAsync();
    public Task<Book?> GetByIdAsync(int id);
    public Task<Book> CreateAsync(BookRequest bookRequest);
    public Task<Book?> UpdateAsync(int id, BookRequest bookRequest);
}
public class BookService(LibraryDbContext dbContext, ILogger<BookService> logger) : IBookService
{
    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await dbContext.Books.ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await dbContext.Books.FindAsync(id);
    }

    public async Task<Book> CreateAsync(BookRequest bookRequest)
    {
        var book = new Book()
        {
            Title = bookRequest.Title,
            Author = bookRequest.Author,
            Isbn = bookRequest.Isbn,
        };

        var publishedDate = CheckPublishedDate(bookRequest);
        book.PublishedDate = publishedDate;

        await dbContext.Books.AddAsync(book);
        await dbContext.SaveChangesAsync();

        return book;
    }

    public async Task<Book?> UpdateAsync(int id, BookRequest bookRequest)
    {
        var existingBook = await dbContext.Books.FindAsync(id);

        if (existingBook == null)
        {
            logger.LogWarning("Book to update does not exists.");
            return null;
        }

        existingBook.Title = bookRequest.Title;
        existingBook.Author = bookRequest.Author;
        existingBook.Isbn = bookRequest.Isbn;

        var publishedDate = CheckPublishedDate(bookRequest);
        existingBook.PublishedDate = publishedDate;

        dbContext.Books.Update(existingBook);
        await dbContext.SaveChangesAsync();

        return existingBook;
    }

    private DateOnly CheckPublishedDate(BookRequest bookRequest)
    {
        if (!DateOnly.TryParse(bookRequest.PublishedDate, out var publishedDate))
        {
            var errorMessage = $"The provided Published Date '{bookRequest.PublishedDate}' is not a valid date format.";
            logger.LogError(errorMessage);
            throw new InvalidPublishedDateException(errorMessage);
        }

        return publishedDate;
    }
}
