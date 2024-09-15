using Microsoft.Extensions.Logging;
using ZigZag.Library.API.Services.Exceptions;
using ZigZag.Library.API.Services.Services;
using ZigZag.Library.DataAccess.Models;
using ZigZag.Library.DataAccess.Models.Requests;

namespace ZigZag.Library.Tests;

public class BookServiceTests : InMemoryTestBase
{
    private readonly Mock<ILogger<BookService>> _mockLogger = new();

    private BookService CreateSut() => new(Context, _mockLogger.Object);

    private async Task SetupBooks()
    {
        Context.Books.AddRange(new List<Book>
            {
                new() { Id =1, Author = "Test Author", Title = "Test Book 1", Isbn = "Test ISBN", PublishedDate = DateOnly.Parse("09/06/2024")},
                new() { Id =2, Author = "Test Author", Title = "Test Book 2", Isbn = "Test ISBN", PublishedDate = DateOnly.Parse("09/06/2024")},
            });

        await Context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllBooks()
    {
        await SetupBooks();

        var sut = CreateSut();
        var result = await sut.GetAllAsync();

        var listOfBooks = result.ToList();
        listOfBooks.Should().NotBeNull();
        listOfBooks.Should().HaveCount(2);
    }

    [Theory]
    [InlineData(1, "Test Book 1")]
    [InlineData(2, "Test Book 2")]
    public async Task GetByIdAsync_ShouldReturnBook_WhenBookExists(int id, string expectedTitle)
    {
        await SetupBooks();

        var sut = CreateSut();
        var result = await sut.GetByIdAsync(id);

        result.Should().NotBeNull();
        result!.Title.Should().Be(expectedTitle);
    }

    [Theory]
    [InlineData(99)]
    [InlineData(100)]
    public async Task GetByIdAsync_ShouldReturnNull_WhenBookDoesNotExist(int id)
    {
        await SetupBooks();

        var sut = CreateSut();
        var result = await sut.GetByIdAsync(id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldAddNewBook()
    {
        await SetupBooks();

        var newBook = new BookRequest
        {
            Title = "New Test Book",
            Author = "New Author",
            Isbn = "1234567890",
            PublishedDate = "2023-09-01"
        };

        var sut = CreateSut();
        var result = await sut.CreateAsync(newBook);

        result.Should().NotBeNull();
        result.Title.Should().Be("New Test Book");
        result.Author.Should().Be("New Author");
        result.Isbn.Should().Be("1234567890");
        var expectedDate = DateOnly.Parse(newBook.PublishedDate);
        result.PublishedDate.Should().Be(expectedDate);
        Context.Books.Should().HaveCount(3);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidPublishedDateException_ForInvalidDateFormat()
    {
        await SetupBooks();

        var newBookWithInvalidDate = new BookRequest
        {
            Title = "New Test Book",
            Author = "New Author",
            Isbn = "1234567890",
            PublishedDate = "invalid-date"
        };

        var sut = CreateSut();

        Func<Task> act = async () => await sut.CreateAsync(newBookWithInvalidDate);

        await act.Should().ThrowAsync<InvalidPublishedDateException>()
            .WithMessage("The provided Published Date 'invalid-date' is not a valid date format.");

        Context.Books.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateExistingBook()
    {
        await SetupBooks();
        var updatedBook = new BookRequest
        {
            Title = "Updated Test Book",
            Author = "Updated Author",
            Isbn = "0987654321",
            PublishedDate = "2023-09-02"
        };

        var sut = CreateSut();
        var result = await sut.UpdateAsync(1, updatedBook);

        result.Should().NotBeNull();
        result?.Title.Should().Be("Updated Test Book");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowInvalidPublishedDateException_ForInvalidDateFormat()
    {
        await SetupBooks();
        var updatedBook = new BookRequest
        {
            Title = "Updated Test Book",
            Author = "Updated Author",
            Isbn = "0987654321",
            PublishedDate = "invalid-date"
        };

        var sut = CreateSut();

        Func<Task> act = async () => await sut.UpdateAsync(1, updatedBook);

        await act.Should().ThrowAsync<InvalidPublishedDateException>()
            .WithMessage("The provided Published Date 'invalid-date' is not a valid date format.");
    }

    [Theory]
    [InlineData(99)]
    [InlineData(100)]
    public async Task UpdateAsync_ShouldReturnNull_WhenBookDoesNotExist(int id)
    {
        await SetupBooks();
        var updatedBook = new BookRequest
        {
            Title = "Non-existent Book",
            Author = "Non-existent Author",
            Isbn = "0000000000",
            PublishedDate = "2023-09-02"
        };

        var sut = CreateSut();
        var result = await sut.UpdateAsync(id, updatedBook);

        result.Should().BeNull();
    }
}
