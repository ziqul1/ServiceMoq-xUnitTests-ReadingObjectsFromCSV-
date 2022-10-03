using CsvHelper;
using CsvHelper.Configuration;
using MojeAPI.Models;
using System.Globalization;

namespace MojeAPI.Data.Services
{
    public class FileService : IFileService
    {
        private readonly IBookService _bookService;

        public FileService(IBookService bookService)
            => _bookService = bookService;

        public async Task<bool> ReadFile(IFormFile filePath)
        {
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ";"
            };

            var isCSVFile = filePath.FileName.Substring(filePath.FileName.Length - 3);

            if (isCSVFile == "csv")
            {
                using (var reader = new StreamReader(filePath.OpenReadStream()))
                using (var csvReader = new CsvReader(reader, config))
                {
                    var objects = csvReader.GetRecords<BookDTO>().ToList();

                    foreach (var record in objects)
                    {
                        await _bookService.CreateBookAsync(record);
                    }

                    return true;
                }
            }
            else
                return false;
        }

        public async Task<byte[]> WriteFile(int numberOfRecordsToTake, int skip, string filteredBooks)
        {
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                Delimiter = ";"
            };

            var memoryStream = new MemoryStream();

            using (var writer = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(writer, config))
            {
                csvWriter.WriteRecords(await _bookService.FilterBooks(numberOfRecordsToTake, skip, filteredBooks));
            }

            return memoryStream.ToArray();
        }
    }
}
