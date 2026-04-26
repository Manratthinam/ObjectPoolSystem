using Microsoft.Extensions.Logging;
using ObjectPoolSystem.Application.Interface;
using ObjectPoolSystem.Domain.Pools;
using ObjectPoolSystem.Infrastructure.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Infrastructure.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly ObjectPool<PoolSmtpClient> _smtpPool;
        private readonly ILogger<EmailService> _logger;

        public EmailService(ObjectPool<PoolSmtpClient> smtpPool, ILogger<EmailService> logger)
        {
            _smtpPool = smtpPool;
            _logger = logger;
        }
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var clent = await _smtpPool.AcquireAsync();
            try
            {
                clent.SendEmail(to, subject, body);
                _logger.LogInformation($"Email sent to {to} with subject '{subject}'");
            }
            finally
            {
                _smtpPool.Release(clent);
            }
        }
    }
}
