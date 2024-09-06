using Microsoft.EntityFrameworkCore;
using ZigZag.Library.DataAccess.Models;

namespace ZigZag.Library.DataAccess
{
    public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
    }
}
