using Microsoft.EntityFrameworkCore;

namespace MojeAPI.Models
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("BooksConnectionString");
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
