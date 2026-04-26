using ObjectPoolSystem.Application.Interface;
using ObjectPoolSystem.Domain.Interfaces;
using ObjectPoolSystem.Domain.Pools;
using Microsoft.Extensions.Logging;

namespace ObjectPoolSystem.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly ObjectPool<IPoolSmtpConnection> _smtpPool;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ObjectPool<IPoolSmtpConnection> smtpPool, ILogger<EmailService> logger)
        {
            _smtpPool = smtpPool;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var client = await _smtpPool.AcquireAsync();

            try
            {
                client.SendEmail(to, subject, body);
                _logger.LogInformation("Email sent successfully to {To}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
                throw;
            }
            finally
            {
                _smtpPool.Release(client);
            }
        }
    }
}
