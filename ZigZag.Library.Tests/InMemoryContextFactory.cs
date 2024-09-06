using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using ZigZag.Library.DataAccess;

namespace ZigZag.Library.Tests
{
    public class InMemoryContextFactory : IAsyncLifetime
    {
        private DbConnection _connection = null!;

        public Task InitializeAsync()
        {
            _connection = CreateInMemoryDatabase();
            return Task.CompletedTask;
        }

        public LibraryDbContext GetContext()
        {
            if (_connection == null)
            {
                throw new ApplicationException(
                    $"{nameof(InMemoryContextFactory)} must be initialized before use. Please call InitializeAsync.");
            }

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseSqlite(_connection) 
                .Options;

            var context = new LibraryDbContext(options);
            context.Database.EnsureCreated(); 

            return context;
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

        public async Task DisposeAsync()
        {
            if (_connection != null)
            {
                await _connection.DisposeAsync();
            }
        }
    }
}