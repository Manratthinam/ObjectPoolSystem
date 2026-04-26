using Microsoft.AspNetCore.Mvc;
using ObjectPoolSystem.Application.Interface;
using ObjectPoolSystem.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace ObjectPoolSystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IEmailService _emailService;

        public DataController(IDatabaseService databaseService, IEmailService emailService)
        {
            _databaseService = databaseService;
            _emailService = emailService;
        }

        [HttpPost("query")]
        public async Task<IActionResult> Query()
        {
            await _databaseService.ExecuteQueryAsync();
            return Ok(new { message = "Query executed successfully." });
        }

        [HttpPost("stress")]
        public IActionResult Stress()
        {
            for (int i = 0; i < 20; i++)
            {
                Task.Run(async () =>
                {
                    await _databaseService.ExecuteQueryAsync();
                });
                Task.Run(async () =>
                {
                    await _emailService.SendEmailAsync(
                        to: $"example@gmail.com",subject: $"Test Email {i}", body: $"This is a test email number {i}."
                    );
                } );
            }
            return Accepted(new { message = "Stress test started." });
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendNotification([FromBody] EmailRequest email)
        {
            await _emailService.SendEmailAsync(
                to: email.To,
                subject: email.Subject,
                body: email.Body
            );

            return Ok("Email sent successfully");
        }
    }

    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
