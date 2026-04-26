namespace ObjectPoolSystem.Domain.Interfaces
{
    public interface IPoolSmtpConnection : IPoolable
    {
        void SendEmail(string to, string subject, string body);
    }
}
