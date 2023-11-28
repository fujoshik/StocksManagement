namespace Accounts.Domain.Abstraction.Services
{
    public interface IEmailService
    {
        void SendEmail(string email, string text);
    }
}
