using ZigZag.Library.DataAccess;

namespace ZigZag.Library.Tests;

public class InMemoryTestBase : IAsyncLifetime
{
    private readonly InMemoryContextFactory _factory = new();
    public LibraryDbContext Context = null!;

    public async Task InitializeAsync()
    {
        await _factory.InitializeAsync();
        Context = _factory.GetContext();
    }

    public async Task DisposeAsync()
    {
        await Context.DisposeAsync();
        await _factory.DisposeAsync();
    }
}