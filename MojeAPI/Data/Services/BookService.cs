using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojeAPI.Models;

namespace MojeAPI.Data.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _libraryContext;

        public BookService(LibraryContext libraryContext)
            => _libraryContext = libraryContext;

        public async Task<IEnumerable<BookDTO>> GetBooksAsync()
        {
            return await _libraryContext.Books
                .Select(x => BookToDTO(x))
                .ToListAsync();
        }

        public async Task<BookDTO> GetSingleBookAsync(int id)
        {
            var book = await _libraryContext.Books.FindAsync(id);

            return BookToDTO(book);
        }

        public async Task<long> UpdateBookAsync(int id, BookDTO bookDTO)
        {
            var book = await _libraryContext.Books.FindAsync(id);

            book.Title = bookDTO.Title;
            book.Price = bookDTO.Price;

            return await _libraryContext.SaveChangesAsync();
        }

        public async Task<BookDTO> CreateBookAsync(BookDTO bookDTO)
        {
            var book = new Book
            {
                Title = bookDTO.Title,
                Price = bookDTO.Price,
            };

            _libraryContext.Books.Add(book);
            await _libraryContext.SaveChangesAsync();

            return BookToDTO(book);
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _libraryContext.Books.FindAsync(id);

            _libraryContext.Books.Remove(book);
            if (await _libraryContext.SaveChangesAsync() != 1)
                return false;

            return true;
        }

        public async Task<List<BookDTO>> FilterBooks(int numberOfRecordsToTake, int skip, string filteredBooks)
        {
            var queryable = _libraryContext.Books.AsQueryable();

            if (filteredBooks != null)
                queryable = queryable.Where(x => x.Title.Contains(filteredBooks));

            if (numberOfRecordsToTake > 0)
                queryable = queryable.Take(numberOfRecordsToTake);

            if (skip > 0)
                queryable = queryable.Skip(skip);

            return await queryable.Select(x => BookToDTO(x)).ToListAsync();
        }

        private static BookDTO BookToDTO(Book book) =>
            new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                Price = book.Price,
            };
    }
}
