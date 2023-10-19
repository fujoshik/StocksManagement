namespace Accounts.Domain.Abstraction.Services
{
    public interface IPasswordService
    {
        string HashPasword(string password, byte[] salt);
        byte[] GenerateSalt();
        bool VerifyPassword(string password, string hash, byte[] salt);
    }
}
