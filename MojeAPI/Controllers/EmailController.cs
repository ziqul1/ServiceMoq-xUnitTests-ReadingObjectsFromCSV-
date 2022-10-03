using EmailServiceV2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace MojeAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IFeatureManager _featureManager;

        public EmailController(IEmailService emailService, IFeatureManager featureManager)
        {
            _emailService = emailService;
            _featureManager = featureManager;
        }

        [HttpPost]
        public async Task Index(IFormFile filePath)
        {
            Boolean isSendingEnabled = await _featureManager.IsEnabledAsync("isSendingEnabled");

            var files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();

            var message = new Message(
                new string[] { "kpjoter56@gmail.com" },
                "Test email",
                "MAMY TO !",
                files
                );

            if (isSendingEnabled)
                _emailService.SendEmail(message);
        }
    }
}
