using Microsoft.AspNetCore.Mvc;
using System.IO;
using CsvHelper;
using MojeAPI.Models;
using System.Globalization;
using CsvHelper.Configuration;
using MojeAPI.Data.Services;
using EmailServiceV2;
using Microsoft.FeatureManagement;

namespace MojeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly IFeatureManager _featureManager;

        public FileController(IFileService fileService, IEmailService emailService, IFeatureManager featureManager)
        {
            _fileService = fileService;
            _emailService = emailService;
            _featureManager = featureManager;
        }

        [HttpPost]
        public async Task<ActionResult> ReadFile(IFormFile filePath)
        {
            Boolean isSendingEnabled = await _featureManager.IsEnabledAsync("isSendingEnabled");
            var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();

            var message = new Message(
                new string[] { "kpjoter56@gmail.com" },
                "Test email",
                "This is the content from our email.",
                files
                );

            if (isSendingEnabled)
                _emailService.SendEmail(message);

            if (await _fileService.ReadFile(filePath))
                return Ok();
            else
                return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<BookDTO>> WriteFile(int numberOfRecordsToTake, int skip, string filteredBooks)
        {
            var bytes = await _fileService.WriteFile(numberOfRecordsToTake, skip, filteredBooks);

            return File(bytes, "application/json", Path.GetFileName("newBooks.csv"));
        }
    }
}
