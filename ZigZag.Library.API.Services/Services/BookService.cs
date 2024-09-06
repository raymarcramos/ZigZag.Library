using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZigZag.Library.DataAccess;
using ZigZag.Library.DataAccess.Models;
using ZigZag.Library.DataAccess.Models.Dto;

namespace ZigZag.Library.API.Services.Services
{
    public interface IBookService
    {
        public Task<IEnumerable<Book>> GetAllAsync();
        public Task<Book?> GetByIdAsync(int id);
        public Task<Book> CreateAsync(BookDto book);
        public Task<Book?> UpdateAsync(int id, BookDto book);
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

        public async Task<Book> CreateAsync(BookDto bookDto)
        {
            var book = new Book()
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Isbn = bookDto.Isbn,
                PublishedDate = bookDto.PublishedDate
            };

            await dbContext.Books.AddAsync(book);

            await dbContext.SaveChangesAsync();

            return book;
        }

        public async Task<Book?> UpdateAsync(int id, BookDto book)
        {
            var existingBook = await dbContext.Books.FindAsync(id);

            if (existingBook == null)
            {
                logger.LogWarning("Book to update does not exists.");
                return null;
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Isbn = book.Isbn;
            existingBook.PublishedDate = book.PublishedDate;

            dbContext.Books.Update(existingBook);
            await dbContext.SaveChangesAsync();

            return existingBook;
        }
    }
}
