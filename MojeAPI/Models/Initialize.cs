using Microsoft.EntityFrameworkCore;

namespace MojeAPI.Models
{
    public class Initialize
    {
        public static LibraryContext GetContext()
        {
            DbContextOptionsBuilder<LibraryContext> options = new DbContextOptionsBuilder<LibraryContext>();
            options.UseSqlServer("BooksConnectionString");
            return new LibraryContext(options.Options);
        }
    }
}
