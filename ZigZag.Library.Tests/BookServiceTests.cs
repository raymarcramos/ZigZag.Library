using Microsoft.Extensions.Logging;
using ZigZag.Library.API.Services.Services;
using ZigZag.Library.DataAccess.Models;
using ZigZag.Library.DataAccess.Models.Dto;

namespace ZigZag.Library.Tests
{
    public class BookServiceTests() : InMemoryTestBase
    {
        private readonly Mock<ILogger<BookService>> _mockLogger = new();

        private BookService CreateSut() => new(Context, _mockLogger.Object);

        private void SetupBooks() => Context.Books.AddRange(new List<Book>
        {
            new() { Id =1, Author = "Test Author", Title = "Test Book 1", Isbn = "Test ISBN", PublishedDate = "09/06/2024"},
            new() { Id =1, Author = "Test Author", Title = "Test Book 2", Isbn = "Test ISBN", PublishedDate = "09/01/2024"},
        });

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooks()
        {
            SetupBooks();

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
            SetupBooks();

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
            SetupBooks();

            var sut = CreateSut();
            var result = await sut.GetByIdAsync(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewBook()
        {
            SetupBooks();

            var newBook = new BookDto
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
            Context.Books.Should().HaveCount(3);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExistingBook()
        {
            SetupBooks();
            var updatedBook = new BookDto
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

        [Theory]
        [InlineData(99)]
        [InlineData(100)]
        public async Task UpdateAsync_ShouldReturnNull_WhenBookDoesNotExist(int id)
        {
            SetupBooks();
            var updatedBook = new BookDto
            {
                Title = "Non-existent Book",
                Author = "Non-existent Author",
                Isbn = "0000000000",
                PublishedDate = "2023-09-02"
            };

            var sut = CreateSut();
            var result = await sut.UpdateAsync(id, updatedBook);

            result.Should().BeNull();
            _mockLogger.Verify(log => log.LogWarning("Book to update does not exists."), Times.Once);
        }
    }
}
