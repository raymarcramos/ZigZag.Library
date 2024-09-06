using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ZigZag.Library.DataAccess;

public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    public LibraryDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<LibraryDbContext>();
        const string connectionString = "Server=localhost;Database=ZigzagDB;User Id=sa;Password=p@ssw0rd;TrustServerCertificate=True;";

        builder.UseSqlServer(connectionString);

        return new LibraryDbContext(builder.Options);
    }
}