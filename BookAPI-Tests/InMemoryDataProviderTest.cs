using Microsoft.EntityFrameworkCore;
using MojeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MojeAPI.Data.Services;
using FluentAssertions;

namespace BookAPI_Tests
{
    public class InMemoryDataProviderTest
    {
        private readonly LibraryContext _context;
        private readonly DbContextOptions<LibraryContext> _contextOptions;

        public InMemoryDataProviderTest()
        {
            _contextOptions = new DbContextOptionsBuilder<LibraryContext>()
           .UseInMemoryDatabase(Guid.NewGuid().ToString())
           .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
           .Options;

            _context = new LibraryContext(_contextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.AddRange(
                 new Book { Title = "Ognisko u Karola", Price = 69 },
                 new Book { Title = "Ostre chlanie", Price = 25 }
                );

            _context.SaveChanges();
        }

        [Fact]
        public async void GetBooksAsync()
        {
            //Arrange
            var service = new BookService(_context);

            //Act
            var listOfBooks = await service.GetBooksAsync();
            var expectedFromDB = await _context.Books
                .Select(x => BookToDTO(x))
                .ToListAsync();

            //Assert
            listOfBooks.Should().NotBeNull();
            expectedFromDB.Should().NotBeNull();
            expectedFromDB.Should().BeEquivalentTo(listOfBooks);
        }

        [Fact]
        public async void GetSingleBookAsync()
        {
            //Arrange
            var service = new BookService(_context);

            //Act
            var book = await service.GetSingleBookAsync(2);
            var expectedFromDB = await _context.Books.FindAsync(2);

            //Assert
            book.Should().NotBeNull();
            expectedFromDB.Should().NotBeNull();
            expectedFromDB.Should().BeEquivalentTo(book);
        }

        [Fact]
        public async void UpdateBookAsync()
        {
            //Arrange
            var service = new BookService(_context);
            var tempBook = new BookDTO
            {
                Id = 2,
                Title = "szybkie dziewczyny i fajne samochody",
                Price = 69
            };

            //Act
            await service.UpdateBookAsync(2, tempBook);
            var expectedFromDB = await _context.Books
                .FirstOrDefaultAsync(x => x.Price == tempBook.Price
                                    && x.Title == tempBook.Title);

            //Assert
            expectedFromDB.Should().NotBeNull();
            expectedFromDB.Title.Should().Be(tempBook.Title);
            expectedFromDB.Price.Should().Be(tempBook.Price);
        }

        [Fact]
        public async void CreateBookAsync()
        {
            //Arrange
            var service = new BookService(_context);
            var tempBook = new BookDTO
            {
                Title = "szybkie dziewczyny i fajne samochody",
                Price = 69
            };

            //Act
            var book = await service.CreateBookAsync(tempBook);
            var expectedFromDB = await _context.Books
                .FirstOrDefaultAsync(x => x.Price == tempBook.Price
                                    && x.Title == tempBook.Title);

            //Assert
            book.Should().NotBeNull();
            expectedFromDB.Should().NotBeNull();
            book.Price.Should().Be(tempBook.Price);
            book.Title.Should().Be(tempBook.Title);
        }

        [Fact]
        public async void DeleteBookAsync()
        {
            //Arrange
            var service = new BookService(_context);

            //Act
            await service.DeleteBookAsync(2);
            var expectedFromDB = await _context.Books
                .FirstOrDefaultAsync(x => x.Id == 2);

            //Assert
            expectedFromDB.Should().BeNull();
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
