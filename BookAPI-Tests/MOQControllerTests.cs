using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MojeAPI.Controllers;
using MojeAPI.Data.Services;
using MojeAPI.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAPI_Tests
{
    public class MOQControllerTests
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BooksDTOController _controller;

        public MOQControllerTests()
        {
            _mockBookService = new Mock<IBookService>();
            _controller = new BooksDTOController(_mockBookService.Object);
        }

        [Fact]
        public async void GetBooksAsync()
        {
            _mockBookService.Setup(repo => repo.GetBooksAsync())
                .ReturnsAsync(new List<BookDTO>() {
                    new BookDTO() { Id = 1, Title = "Shrek", Price = 69 },
                    new BookDTO() { Id = 2, Title = "Transformers", Price = 59 },
                });

            var books = await _controller.GetBooksAsync();
            books.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async void GetSingleBookAsync()
        {
            _mockBookService.Setup(repo => repo.GetSingleBookAsync(1))
                .ReturnsAsync(new BookDTO() { Id = 1, Title = "Shrek", Price = 69 });

            var book = await _controller.GetSingleBookAsync(1);
            book.Value.Should().BeEquivalentTo(await _mockBookService.Object.GetSingleBookAsync(1));
        }

        [Fact]
        public async void UpdateBookAsync()
        {
            var tempBook = new BookDTO()
            {
                Id = 1,
                Title = "szybkie dziewczyny i fajne samochody",
                Price = 69
            };

            _mockBookService.Setup(repo => repo.GetSingleBookAsync(1)).ReturnsAsync(tempBook);
            _mockBookService.Setup(repo => repo.UpdateBookAsync(1, tempBook)).ReturnsAsync(1);

            var book = await _controller.UpdateBookAsync(1, tempBook);

            book.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async void CreateBookAsync()
        {
            var tempBook = new BookDTO
            {
                Id = 9,
                Title = "szybkie dziewczyny i fajne samochody",
                Price = 69
            };

            _mockBookService.Setup(repo => repo.CreateBookAsync(tempBook))
                .ReturnsAsync(tempBook);
            
            var book = await _controller.CreateBookAsync(tempBook);
            
            book.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async void DeleteBookAsync()
        {
            var tempBook = new BookDTO
            {
                Id = 9,
                Title = "szybkie dziewczyny i fajne samochody",
                Price = 69
            };

            _mockBookService.Setup(repo => repo.GetSingleBookAsync(1)).ReturnsAsync(tempBook);
            _mockBookService.Setup(repo => repo.DeleteBookAsync(1)).ReturnsAsync(true);
                
            var result = await _controller.DeleteBookAsync(1);

            // if(result == NoContentResult())

            result.Should().BeOfType<NoContentResult>();
        }
    }
}
