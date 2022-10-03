using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MojeAPI.Models;

namespace MojeAPI.Data.Services
{
    public interface IBookService
    {
        public Task<IEnumerable<BookDTO>> GetBooksAsync();
        public Task<BookDTO> GetSingleBookAsync(int id);
        public Task<long> UpdateBookAsync(int id, BookDTO bookDTO);
        public Task<BookDTO> CreateBookAsync(BookDTO bookDTO);
        public Task<bool> DeleteBookAsync(int id);
        public Task<List<BookDTO>> FilterBooks(int numberOfRecordsToTake, int skip, string filteredBooks);
    }
}
