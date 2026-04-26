using ObjectPoolSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ObjectPoolSystem.Infrastructure.Resources
{
    public class PoolSmtpClient : IPoolSmtpConnection
    {
        private readonly SmtpClient _smtpClient;
        private bool _isvalid = true;

        public PoolSmtpClient(string host, int port, string username, string password)
        {
            _smtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true,
            };
        }
        public DateTime CreatedAt => DateTime.UtcNow;

        public DateTime LastUsedAt { get; set; } = DateTime.UtcNow;

        public void SendEmail(string to, string subject, string body)
        {
            var mail = new MailMessage("noreply@myapp.com", to, subject, body);
            _smtpClient.Send(mail);
        }

        public void Dispose()
        {
            _isvalid = false;
            _smtpClient.Dispose();
        }

        public bool IsValid()
        {
            return _isvalid;
        }

        public void Reset()
        {
            _isvalid = true;
        }
    }
}
